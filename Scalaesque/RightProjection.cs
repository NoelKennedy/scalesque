using System;

namespace Scalesque {

    /// <summary>
    /// A projection of the Right side of an Either&lt;T,UU&gt;.  Provides methods for manipulating the potential right value.
    /// </summary>
    /// <typeparam name="T">T The type of the left side value</typeparam>
    /// <typeparam name="U">U The type of the right side value</typeparam>
    public sealed class RightProjection<T,U>
    {
        private readonly Either<T, U> either;
        private readonly Func<U> getRight;

        public RightProjection(Either<T,U> either, Func<U> getRight) {
            this.either = either;
            this.getRight = getRight;
        }

        /// <summary>
        /// Gets the value of the Right
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if called on a Left&lt;T,U&gt;</exception>
        /// <returns></returns>
        public U Get() {
            return getRight();
        }

        /// <summary>
        /// Gets an optional Right value
        /// </summary>
        /// <returns>Some&lt;U,U&gt; if Right, else None&lt;U,U&gt;</returns>
        public Option<U> ToOption() {
            return either.Fold(left => Option.None(), right => Option.Some(right));
        }

        /// <summary>
        /// Performs a side effect on the Right value
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<U> action) {
            if (either.IsRight)
                action(getRight());
        }

        /// <summary>
        /// Maps through the Right side
        /// </summary>
        /// <typeparam name="Y">New type of the Right</typeparam>
        /// <param name="f">Func&lt;U,Y&gt; A function to convert U to a Y</param>
        /// <returns></returns>
        public Either<T,Y> Map<Y>(Func<U,Y> f) {
            if (either.IsRight)
                return Either.Right(f(getRight()));

            return Either.Left(either.ProjectLeft().Get());
        }

        /// <summary>
        /// Flattens and maps through Right side
        /// </summary>
        /// <typeparam name="Y"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Either<T,Y> FlatMap<Y>(Func<U, Either<T,Y>> f) {
            if (either.IsRight)
                return f(getRight());
            return Either.Left(either.ProjectLeft().Get());
        }

        /// <summary>
        /// Gets the value of the Right or converts the left to U
        /// </summary>
        /// <param name="or">Function to convert a T to a U</param>
        /// <returns></returns>
        public U GetOrElse(Func<T, U> or) {
            if (either.IsRight)
                return getRight();
            return or(either.ProjectLeft().Get());
        }
    }
}

