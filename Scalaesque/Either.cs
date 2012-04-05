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
        public abstract void ForEach(Action<T> leftAction, Action<U> rightAction);

        /// <summary>
        /// Unifies the disjoint into a {A}
        /// </summary>
        /// <typeparam name="T">A The unified type</typeparam>
        /// <param name="foldLeft">Func{T,A} Takes a Left{T} and converted to a A</param>
        /// <param name="foldRight">Func{U,T} Takes a Right{U} and converted to a A</param>
        /// <returns>A The unified type</returns>
        public abstract A Fold<A>(Func<T, A> foldLeft, Func<U, A> foldRight);

        /// <summary>
        /// Maps the right side of the Either
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public abstract Either<T, V> Map<V>(Func<U, V> f);
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

        public override void ForEach(Action<T> leftAction, Action<U> rightAction) {
            leftAction(left);
        }

        public override A Fold<A>(Func<T, A> foldLeft, Func<U, A> foldRight) {
            return foldLeft(left);
        }

        public override Either<T, V> Map<V>(Func<U, V> f) {
            return Either.Left(left);
        }

        public T Get() {
            return left;
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

        public override void ForEach(Action<T> leftAction, Action<U> rightAction) {
            rightAction(right);
        }
        public override A Fold<A>(Func<T, A> foldLeft, Func<U, A> foldRight) {
            return foldRight(right);
        }

        public override Either<T, V> Map<V>(Func<U, V> f) {
            return Either.Right(f(right));
        }

        public U Get() {
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
