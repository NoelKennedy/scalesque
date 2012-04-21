using System;
using System.Collections.Generic;
using System.Linq;
using Scalesque.Collections;

namespace Scalesque {

    /// <summary>
    /// A monad representing either success or failure during validation
    /// </summary>
    /// <typeparam name="T">T The type of failure</typeparam>
    /// <typeparam name="U">U The type of success</typeparam>
    public abstract partial class Validation<T, U> {

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
        /// Lifts the Validation so it can be combined with another instance
        /// </summary>
        /// <returns></returns>
        public Validation<NonEmptySList<T>, Tuple<U>> Lift() {
            return Fold<Validation<NonEmptySList<T>, Tuple<U>>>(
                fail => new NonEmptySList<T>(fail).ToFailure(), 
                success => Tuple.Create(success).ToSuccess());
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
        /// Combines two Validation instances to form a single validation.  If either Validation is a Failure, the result is also a Failure.
        /// </summary>
        /// <remarks>Failures are accumulated in a <see cref="NonEmptySList{T}"/>.  Success instances are accumulated in a Tuple</remarks>
        /// <returns>Validation&lt;NonEmptySList&lt;T&gt;, Tuple&lt;U, Y&gt;&gt;</returns>
        public static Validation<NonEmptySList<T>, Tuple<U, Y>> Combine<T, U, Y>(this Validation<NonEmptySList<T>, Tuple<U>> first, Validation<NonEmptySList<T>, Tuple<Y>> other) {
            if (first.IsFailure || other.IsFailure) {
                IEnumerable<NonEmptySList<T>> listOfLists = first.ProjectFailure().Concat(other.ProjectFailure());
                IEnumerable<T> flattened = CollectionExtensions.Flatten(listOfLists);
                return SList.apply(flattened).ToNonEmptyList().Get().ToFailure();
            }
            return Tuple.Create(first.ProjectSuccess().Get().Item1, other.ProjectSuccess().Get().Item1).ToSuccess();
        }


        /// <summary>
        /// Combines two Validation instances to form a single validation.  If either Validation is a Failure, the result is also a Failure.
        /// </summary>
        /// <remarks>Failures are accumulated in a <see cref="NonEmptySList{T}"/>.  Success instances are accumulated in a Tuple</remarks>
        /// <returns>Validation&lt;NonEmptySList&lt;T&gt;, Tuple&lt;U, Y&gt;&gt;</returns>
        public static Validation<NonEmptySList<T>, Tuple<U, Y, A>> Combine<T, U, Y, A>(this Validation<NonEmptySList<T>, Tuple<U,Y>> first, Validation<NonEmptySList<T>, Tuple<A>> other) {
            if (first.IsFailure || other.IsFailure) {
                IEnumerable<NonEmptySList<T>> listOfLists = first.ProjectFailure().Concat(other.ProjectFailure());
                IEnumerable<T> flattened = listOfLists.Flatten();
                return SList.apply(flattened).ToNonEmptyList().Get().ToFailure();
            }
            var firstSuccess = first.ProjectSuccess().Get();
            return Tuple.Create(firstSuccess.Item1, firstSuccess.Item2, other.ProjectSuccess().Get().Item1).ToSuccess();
        }

        /// <summary>
        /// Combines two Validation instances to form a single validation.  If either Validation is a Failure, the result is also a Failure.
        /// </summary>
        /// <remarks>Failures are accumulated in a <see cref="NonEmptySList{T}"/>.  Success instances are accumulated in a Tuple</remarks>
        /// <returns>Validation&lt;NonEmptySList&lt;T&gt;, Tuple&lt;U, Y&gt;&gt;</returns>
        public static Validation<NonEmptySList<T>, Tuple<U, Y, A, B>> Combine<T, U, Y, A, B>(this Validation<NonEmptySList<T>, Tuple<U, Y, A>> first, Validation<NonEmptySList<T>, Tuple<B>> other) {
            if (first.IsFailure || other.IsFailure) {
                IEnumerable<NonEmptySList<T>> listOfLists = first.ProjectFailure().Concat(other.ProjectFailure());
                IEnumerable<T> flattened = listOfLists.Flatten();
                return SList.apply(flattened).ToNonEmptyList().Get().ToFailure();
            }
            var firstSuccess = first.ProjectSuccess().Get();
            return Tuple.Create(firstSuccess.Item1, firstSuccess.Item2, firstSuccess.Item3, other.ProjectSuccess().Get().Item1).ToSuccess();
        }

        /// <summary>
        /// Combines two Validation instances to form a single validation.  If either Validation is a Failure, the result is also a Failure.
        /// </summary>
        /// <remarks>Failures are accumulated in a <see cref="NonEmptySList{T}"/>.  Success instances are accumulated in a Tuple</remarks>
        /// <returns>Validation&lt;NonEmptySList&lt;T&gt;, Tuple&lt;U, Y&gt;&gt;</returns>
        public static Validation<NonEmptySList<T>, Tuple<U, Y, A, B, C>> Combine<T, U, Y, A, B, C>(this Validation<NonEmptySList<T>, Tuple<U, Y, A, B>> first, Validation<NonEmptySList<T>, Tuple<C>> other) {
            if (first.IsFailure || other.IsFailure) {
                IEnumerable<NonEmptySList<T>> listOfLists = first.ProjectFailure().Concat(other.ProjectFailure());
                IEnumerable<T> flattened = listOfLists.Flatten();
                return SList.apply(flattened).ToNonEmptyList().Get().ToFailure();
            }
            var firstSuccess = first.ProjectSuccess().Get();
            return Tuple.Create(firstSuccess.Item1, firstSuccess.Item2, firstSuccess.Item3, firstSuccess.Item4, other.ProjectSuccess().Get().Item1).ToSuccess();
        }

        /// <summary>
        /// Combines two Validation instances to form a single validation.  If either Validation is a Failure, the result is also a Failure.
        /// </summary>
        /// <remarks>Failures are accumulated in a <see cref="NonEmptySList{T}"/>.  Success instances are accumulated in a Tuple</remarks>
        /// <returns>Validation&lt;NonEmptySList&lt;T&gt;, Tuple&lt;U, Y&gt;&gt;</returns>
        public static Validation<NonEmptySList<T>, Tuple<U, Y, A, B, C, D>> Combine<T, U, Y, A, B, C, D>(this Validation<NonEmptySList<T>, Tuple<U, Y, A, B, C>> first, Validation<NonEmptySList<T>, Tuple<D>> other) {
            if (first.IsFailure || other.IsFailure) {
                IEnumerable<NonEmptySList<T>> listOfLists = first.ProjectFailure().Concat(other.ProjectFailure());
                IEnumerable<T> flattened = listOfLists.Flatten();
                return SList.apply(flattened).ToNonEmptyList().Get().ToFailure();
            }
            var firstSuccess = first.ProjectSuccess().Get();
            return Tuple.Create(firstSuccess.Item1, firstSuccess.Item2, firstSuccess.Item3, firstSuccess.Item4, firstSuccess.Item5, other.ProjectSuccess().Get().Item1).ToSuccess();
        }

        /// <summary>
        /// Combines two Validation instances to form a single validation.  If either Validation is a Failure, the result is also a Failure.
        /// </summary>
        /// <remarks>Failures are accumulated in a <see cref="NonEmptySList{T}"/>.  Success instances are accumulated in a Tuple</remarks>
        /// <returns>Validation&lt;NonEmptySList&lt;T&gt;, Tuple&lt;U, Y&gt;&gt;</returns>
        public static Validation<NonEmptySList<T>, Tuple<U, Y, A, B, C, D, E>> Combine<T, U, Y, A, B, C, D, E>(this Validation<NonEmptySList<T>, Tuple<U, Y, A, B, C, D>> first, Validation<NonEmptySList<T>, Tuple<E>> other) {
            if (first.IsFailure || other.IsFailure) {
                IEnumerable<NonEmptySList<T>> listOfLists = first.ProjectFailure().Concat(other.ProjectFailure());
                IEnumerable<T> flattened = listOfLists.Flatten();
                return SList.apply(flattened).ToNonEmptyList().Get().ToFailure();
            }
            var firstSuccess = first.ProjectSuccess().Get();
            return Tuple.Create(firstSuccess.Item1, firstSuccess.Item2, firstSuccess.Item3, firstSuccess.Item4, firstSuccess.Item5, firstSuccess.Item6, other.ProjectSuccess().Get().Item1).ToSuccess();
        }

        /// <summary>
        /// Combines two Validation instances to form a single validation.  If either Validation is a Failure, the result is also a Failure.
        /// </summary>
        /// <remarks>Failures are accumulated in a <see cref="NonEmptySList{T}"/>.  Success instances are accumulated in a Tuple</remarks>
        /// <returns>Validation&lt;NonEmptySList&lt;T&gt;, Tuple&lt;U, Y&gt;&gt;</returns>
        public static Validation<NonEmptySList<T>, Tuple<U, Y, A, B, C, D, E, Tuple<F>>> Combine<T, U, Y, A, B, C, D, E, F>(this Validation<NonEmptySList<T>, Tuple<U, Y, A, B, C, D, E>> first, Validation<NonEmptySList<T>, Tuple<F>> other) {
            if (first.IsFailure || other.IsFailure) {
                IEnumerable<NonEmptySList<T>> listOfLists = first.ProjectFailure().Concat(other.ProjectFailure());
                IEnumerable<T> flattened = listOfLists.Flatten();
                return SList.apply(flattened).ToNonEmptyList().Get().ToFailure();
            }
            var firstSuccess = first.ProjectSuccess().Get();
            F otherItem = other.ProjectSuccess().Get().Item1;
            return Tuple.Create(firstSuccess.Item1, firstSuccess.Item2, firstSuccess.Item3, firstSuccess.Item4, firstSuccess.Item5, firstSuccess.Item6, firstSuccess.Item7, otherItem).ToSuccess();
        }
    }
}
