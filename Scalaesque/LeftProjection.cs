using System;

namespace Scalesque {

    /// <summary>
    /// A projection of the Left side of an Either&lt;T,U&gt;.  Provides methods for manipulating the potential left value.
    /// </summary>
    /// <typeparam name="T">T The type of the left side value</typeparam>
    /// <typeparam name="U">U The type of the right side value</typeparam>
    public sealed class LeftProjection<T, U> {
        private readonly Either<T, U> either;
        private readonly Func<T> getLeft;

        public LeftProjection(Either<T, U> either, Func<T> getLeft) {
            this.either = either;
            this.getLeft = getLeft;
        }

        /// <summary>
        /// Gets the value of the Left
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if called on a Right&lt;T,U&gt;</exception>
        /// <returns></returns>
        public T Get() {
            return getLeft();
        }

        /// <summary>
        /// Gets an optional Left value
        /// </summary>
        /// <returns>Some&lt;T,U&gt; if Left, else None&lt;T,U&gt;</returns>
        public Option<T> ToOption() {
            return either.Fold(left => Option.Some(left), right => Option.None());
        }

        /// <summary>
        /// Performs a side effect on the Left value
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action) {
            if (either.IsLeft)
                action(getLeft());
        }

        /// <summary>
        /// Maps through the Left side
        /// </summary>
        /// <typeparam name="Y">New type of the Left</typeparam>
        /// <param name="f">Func&lt;T,Y&gt; A function to convert T to a Y</param>
        /// <returns></returns>
        public Either<Y, U> Map<Y>(Func<T, Y> f) {
            if (either.IsLeft)
                return Either.Left(f(getLeft()));

            return Either.Right(either.ProjectRight().Get());
        }

        /// <summary>
        /// Flattens and maps through Left side
        /// </summary>
        /// <typeparam name="Y"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Either<Y, U> FlatMap<Y>(Func<T, Either<Y, U>> f) {
            if (either.IsLeft)
                return f(getLeft());

            return Either.Right(either.ProjectRight().Get());
        }

        /// <summary>
        /// Gets the value of the Left or converts the right value to T
        /// </summary>
        /// <param name="or">Function to convert a U to a T</param>
        /// <returns></returns>
        public T GetOrElse(Func<U, T> or) {
            if (either.IsLeft)
                return getLeft();
            return or(either.ProjectRight().Get());
        }
    }
}
