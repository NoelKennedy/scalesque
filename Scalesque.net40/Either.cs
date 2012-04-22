using System;

namespace Scalesque {
    
    /// <summary>
    /// Represents a value which can be either one type T, or another type U.  An Either is a Left or a Right.
    /// </summary>
    /// <typeparam name="T">T The type of the left side value</typeparam>
    /// <typeparam name="U">U The type of the right side value</typeparam>
    public abstract partial class Either<T, U> {

        /// <summary>
        /// Performs a side effect on the contents of the Either
        /// </summary>
        /// <param name="leftAction">Action&lt;T&gt; Side effect to perform on a Left&lt;T&gt;</param>
        /// <param name="rightAction">Action&lt;U&gt; Side effect to perform on a Right&lt;U&gt;</param>
        public void ForEach(Action<T> leftAction, Action<U> rightAction) {
            if (IsRight)
                rightAction(GetRight());
            else {
                leftAction(GetLeft());
            }
        }

        /// <summary>
        /// Unifies the disjoint into a &lt;A&gt;
        /// </summary>
        /// <typeparam name="T">A The unified type</typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="foldLeft">Func&lt;T,A&gt; Takes a Left&lt;T&gt; and converted to a A</param>
        /// <param name="foldRight">Func&lt;U,T&gt; Takes a Right&lt;U&gt; and converted to a A</param>
        /// <returns>A The unified type</returns>
        public A Fold<A>(Func<T, A> foldLeft, Func<U, A> foldRight) {
            if (IsRight)
                return foldRight(GetRight());
            else {
                return foldLeft(GetLeft());
            }
        }

        /// <summary>
        /// Gets if this is a Right&lt;T,U&gt;
        /// </summary>
        public abstract bool IsRight { get;}

        /// <summary>
        /// Gets if this is a Left&lt;T,U&gt;
        /// </summary>
        public bool IsLeft {
            get { return !IsRight; }
        }

        /// <summary>
        /// Gets the Left value
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if called on a Right&lt;T,U&gt;</exception>
        /// <returns>T</returns>
        protected abstract T GetLeft();

        /// <summary>
        /// Gets the Right value
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if called on a Left&lt;T,U&gt;</exception>
        /// <returns>U</returns>
        protected abstract U GetRight();

        /// <summary>
        /// Projects right which allows methods for manipulating a potential right value
        /// </summary>
        /// <returns>RightProjection&lt;T,U&gt;</returns>
        public RightProjection<T,U> ProjectRight() {
            return new RightProjection<T, U>(this, GetRight);
        }

        /// <summary>
        /// Projects left which allows methods for manipulating a potential left value
        /// </summary>
        /// <returns> LeftProjection&lt;T,U&gt;</returns>
        public LeftProjection<T,U> ProjectLeft() {
            return new LeftProjection<T, U>(this, GetLeft);
        }
    }

    /// <summary>
    /// Represents the left side of a disjunction.  Conventionally, this is the error side.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public sealed class Left<T, U> : Either<T, U> {
        private readonly T left;
        public Left(T left) {
            this.left = left;
        }

        public override bool IsRight {
            get { return false; }
        }

        protected override T GetLeft() {
            return left;
        }

        protected override U GetRight() {
            throw new NotSupportedException("GetRight() called on Left<T,U>");
        }
    }

    /// <summary>
    /// Represents the right side of a disjunction.  Conventionally, this is the non-error side.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public sealed class Right<T, U> : Either<T, U> {
        private readonly U right;
        public Right(U right) {
           this.right = right;
        }

        public override bool IsRight {
            get { return true; }
        }

        protected override T GetLeft() {
            throw new NotSupportedException("GetLeft() called on Right<T,U>");
        }

        protected override U GetRight() {
            return right;
        }
    }

    /// <summary>
    /// Companion class for Either.  Provides factory methods.
    /// </summary>
    public static class Either {
        
        /// <summary>
        /// Creates a Left&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>Left&lt;T&gt; implicitly convertable to Either&lt;T,U&gt;</returns>
        public static Left<T> Left<T>(T value) {
            return new Left<T>(value);
        }

        /// <summary>
        /// Creates a Right&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="value"></param>
        /// <returns>Right&lt;T&gt; implicitly convertable to Either&lt;T,U&gt;</returns>
        public static Right<U> Right<U>(U value) {
            return new Right<U>(value);
        }

        /// <summary>
        /// Turns a object into a Left&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Left<T> ToLeft<T>(this T value) {
            return Left(value);
        }

        /// <summary>
        /// Turns a object into a Right&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Right<T> ToRight<T>(this T value) {
            return Right(value);
        }

        /// <summary>
        /// Flattens an Either&lt;T,Either&lt;T,U&gt;&gt; through the right side to an Either&lt;T,U&gt;
        /// </summary>
        /// <returns>Either&lt;T,U&gt</returns>
        public static Either<T,U> JoinRight<T,U>(this Either<T,Either<T,U>> either) {
            return either.ProjectRight().FlatMap(x => x);
        }

        /// <summary>
        /// Flattens an Either&lt;Either&lt;T,U&gt;, U&gt; through the left side to an Either&lt;T,U&gt;
        /// </summary>
        /// <returns>Either&lt;T,U&gt</returns>
        public static Either<T,U> JoinLeft<T,U>(this Either<Either<T,U>, U> either){
            return either.ProjectLeft().FlatMap(x => x);
        }
    }
}
