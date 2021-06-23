using System;
using Scalesque.Collections;

namespace Scalesque {
  
    /// <summary>
    /// A monad representing either success or failure during validation
    /// </summary>
    /// <typeparam name="T">T The type of failure</typeparam>
    /// <typeparam name="U">U The type of success</typeparam>
    public abstract partial class Validation<T, U>
    {
        /// <summary>
        /// Performs a side effect on the contents of the Validation
        /// </summary>
        /// <param name="failAction">Action&lt;T&gt; Side effect to perform on a Failure&lt;T&gt;</param>
        /// <param name="successAction">Action&lt;U&gt; Side effect to perform on a Success&lt;U&gt;</param>
        public void ForEach(Action<T> failAction, Action<U> successAction) {
            if (IsSuccess)
                successAction(GetSuccess());
            else {
                failAction(GetFailure());
            }
        }

        /// <summary>
        /// Unifies the disjoint into a &lt;A&gt;
        /// </summary>
        /// <typeparam name="T">A The unified type</typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="foldFailure">Func&lt;T,A&gt; Takes a Failure&lt;T&gt; and converted to a A</param>
        /// <param name="foldSuccess">Func&lt;U,T&gt; Takes a Success&lt;U&gt; and converted to a A</param>
        /// <returns>A The unified type</returns>
        public A Fold<A>(Func<T, A> foldFailure, Func<U, A> foldSuccess) {
            if (IsSuccess)
                return foldSuccess(GetSuccess());
            else {
                return foldFailure(GetFailure());
            }
        }

        /// <summary>
        /// Gets if this is a Success&lt;T,U&gt;
        /// </summary>
        public abstract bool IsSuccess { get; }

        /// <summary>
        /// Gets if this is a Failure&lt;T,U&gt;
        /// </summary>
        public bool IsFailure {
            get { return !IsSuccess; }
        }

        /// <summary>
        /// Gets the Failure value
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if called on a Success&lt;T,U&gt;</exception>
        /// <returns>T</returns>
        protected abstract T GetFailure();

        /// <summary>
        /// Gets the Success value
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if called on a Failure&lt;T,U&gt;</exception>
        /// <returns>U</returns>
        protected abstract U GetSuccess();

        /// <summary>
        /// Projects success which allows methods for manipulating a potential success value
        /// </summary>
        /// <returns>SuccessProjection&lt;T,U&gt;</returns>
        public SuccessProjection<T, U> ProjectSuccess() {
            return new SuccessProjection<T, U>(this, GetSuccess);
        }

        /// <summary>
        /// Projects fail which allows methods for manipulating a potential fail value
        /// </summary>
        /// <returns> FailureProjection&lt;T,U&gt;</returns>
        public FailureProjection<T, U> ProjectFailure()  {
            return new FailureProjection<T, U>(this, GetFailure);
        }

        /// <summary>
        /// Lifts the failure side of the Validation into an non empty list of failures
        /// </summary>
        /// <returns></returns>
        public Validation<ISemiJoin<NonEmptySList<T>>, U> LiftFailNel() {
            return ProjectFailure().Map(x => (ISemiJoin<NonEmptySList<T>>)new NonEmptySList<T>(x));
        }

        /// <summary>
        /// Implements an applicative for Validation&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="TSuccess"></typeparam>
        /// <param name="applicative"></param>
        /// <param name="failApplicative"></param>
        /// <returns></returns>
        public Validation<T, TSuccess> Applicative<TSuccess>(Validation<T, Func<U,TSuccess>> applicative, Func<T,T,T> failApplicative) {


            var matcher = new PatternMatcher<Validation<T, U>, Validation<T, TSuccess>> {
                {Success.unapply, success => applicative.Fold<Validation<T, TSuccess>>(
                    fail => fail.ToFailure(), 
                    successFunc => successFunc(success).ToSuccess())},

                {Failure.unapply, fail1 => applicative.Fold<Validation<T, TSuccess>>(
                    fail2 => failApplicative(fail1, fail2).ToFailure(),
                    successFunc => fail1.ToFailure())}
            };
            return matcher.Get(this).Get();
        }
    }

    /// <summary>
    /// Represents the failure side of a disjunction.  Conventionally, this is the error side.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public sealed class Failure<T, U> : Validation<T, U> {
        private readonly T failure;
        public Failure(T failure) {
            this.failure = failure;
        }

        public override bool IsSuccess {
            get { return false; }
        }

        protected override T GetFailure() {
            return failure;
        }

        protected override U GetSuccess() {
            throw new NotSupportedException("GetSuccess() called on Failure<T,U>");
        }
    }

    /// <summary>
    /// Represents the success side of a disjunction.  Conventionally, this is the non-error side.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public sealed class Success<T, U> : Validation<T, U> {
        private readonly U success;
        public Success(U success) {
            this.success = success;
        }

        public override bool IsSuccess {
            get { return true; }
        }

        protected override T GetFailure() {
            throw new NotSupportedException("GetFailure() called on Success<T,U>");
        }

        protected override U GetSuccess() {
            return success;
        }
    }

    /// <summary>
    /// Companion class for Validation.  Provides factory methods.
    /// </summary>
    public static class Validation {

        /// <summary>
        /// Creates a Failure&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>Failure&lt;T&gt; implicitly convertable to Validation&lt;T,U&gt;</returns>
        public static Failure<T> Failure<T>(T value) {
            return new Failure<T>(value);
        }

        /// <summary>
        /// Creates a Success&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="value"></param>
        /// <returns>Success&lt;T&gt; implicitly convertable to Validation&lt;T,U&gt;</returns>
        public static Success<U> Success<U>(U value) {
            return new Success<U>(value);
        }

        /// <summary>
        /// Turns a object into a Failure&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Failure<T> ToFailure<T>(this T value) {
            return Failure(value);
        }

        /// <summary>
        /// Turns a object into a Success&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Success<T> ToSuccess<T>(this T value) {
            return Success(value);
        }

        /// <summary>
        /// Flattens an Validation&lt;T,Validation&lt;T,U&gt;&gt; through the success side to an Validation&lt;T,U&gt;
        /// </summary>
        /// <returns>Validation&lt;T,U&gt</returns>
        public static Validation<T, U> JoinSuccess<T, U>(this Validation<T, Validation<T, U>> validation) {
            return validation.ProjectSuccess().FlatMap(x => x);
        }


        /// <summary>
        /// Flattens an Validation&lt;T,Validation&lt;T,U&gt;&gt; through the failure side to an Validation&lt;T,U&gt;
        /// </summary>
        /// <returns>Validation&lt;T,U&gt</returns>
        public static Validation<T, U> JoinFailure<T, U>(this Validation<Validation<T, U>, U> validation) {
            return validation.ProjectFailure().FlatMap(x => x);
        }
    }
}
