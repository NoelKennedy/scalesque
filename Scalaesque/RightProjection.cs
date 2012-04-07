using System;

namespace Scalesque {

    public sealed class RightProjection<T,U> {
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

        public U GetOrElse(Func<T, U> or) {
            if (either.IsRight)
                return getRight();
            return or(either.ProjectLeft().Get());
        }
    }
}
