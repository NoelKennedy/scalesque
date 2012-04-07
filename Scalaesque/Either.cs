using System;

namespace Scalesque {
    
    /// <summary>
    /// Represents a disjoint union of two types. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public abstract partial class Either<T, U> {

        /// <summary>
        /// Performs a side effect on the contents of the Either
        /// </summary>
        /// <param name="leftAction">Action{T} Side effect to perform on a Left{T}</param>
        /// <param name="rightAction">Action{U} Side effect to perform on a Right{U}</param>
        public void ForEach(Action<T> leftAction, Action<U> rightAction) {
            if (IsRight)
                rightAction(GetRight());
            else {
                leftAction(GetLeft());
            }
        }

        /// <summary>
        /// Unifies the disjoint into a {A}
        /// </summary>
        /// <typeparam name="T">A The unified type</typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="foldLeft">Func{T,A} Takes a Left{T} and converted to a A</param>
        /// <param name="foldRight">Func{U,T} Takes a Right{U} and converted to a A</param>
        /// <returns>A The unified type</returns>
        public A Fold<A>(Func<T, A> foldLeft, Func<U, A> foldRight) {
            if (IsRight)
                return foldRight(GetRight());
            else {
                return foldLeft(GetLeft());
            }
        }

        /// <summary>
        /// Maps the right side of the Either
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Either<T, V> Map<V>(Func<U, V> f) {
            if(IsRight) 
                return Either.Right(f(GetRight()));
            return Either.Left(GetLeft());
        }

        /// <summary>
        /// Maps the right side of the Either 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Either<T, V> FlatMap<V>(Func<U, Either<T, V>> f) {
            if (IsRight)
                return f(GetRight());
            return
                Either.Left(GetLeft());
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
        public abstract T GetLeft();

        /// <summary>
        /// Gets the Right value
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if called on a Left&lt;T,U&gt;</exception>
        /// <returns>U</returns>
        public abstract U GetRight();
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

        public override T GetLeft() {
            return left;
        }

        public override U GetRight() {
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

        public override T GetLeft() {
            throw new NotSupportedException("GetLeft() called on Right<T,U>");
        }

        public override U GetRight() {
            return right;
        }
    }

    /// <summary>
    /// Companion class for Either.  Provides factory methods.
    /// </summary>
    public static class Either {
        
        /// <summary>
        /// Creates a Left{T,U}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>Left{T} implicitly convertable to Either{T,U}</returns>
        public static Left<T> Left<T>(T value) {
            return new Left<T>(value);
        }

        /// <summary>
        /// Creates a Right{T,U}
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="value"></param>
        /// <returns>Right{T} implicitly convertable to Either{T,U}</returns>
        public static Right<U> Right<U>(U value) {
            return new Right<U>(value);
        }
    }
}
