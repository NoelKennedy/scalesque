using System;

namespace Scalesque {

    /// <summary>
    /// A projection of the Success side of an <see cref="Validation{T,U}"/>.  Provides methods for manipulating the potential success value.
    /// </summary>
    /// <typeparam name="T">T The type of the failure side value</typeparam>
    /// <typeparam name="U">U The type of the success side value</typeparam>
    public sealed class SuccessProjection<T, U> {
        private readonly Validation<T, U> validation;
        private readonly Func<U> getSuccess;

        public SuccessProjection(Validation<T, U> validation, Func<U> getSuccess) {
            this.validation = validation;
            this.getSuccess = getSuccess;
        }

        /// <summary>
        /// Gets the value of the Success
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown if called on a Failure&lt;T,U&gt;</exception>
        /// <returns></returns>
        public U Get() {
            return getSuccess();
        }

        /// <summary>
        /// Gets an optional Success value
        /// </summary>
        /// <returns>Some&lt;U,U&gt; if Success, else None&lt;U,U&gt;</returns>
        public Option<U> ToOption() {
            return validation.Fold(failure => Option.None(), success => Option.Some(success));
        }

        /// <summary>
        /// Performs a side effect on the Success value
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<U> action) {
            if (validation.IsSuccess)
                action(getSuccess());
        }

        /// <summary>
        /// Maps through the Success side
        /// </summary>
        /// <typeparam name="Y">New type of the Success</typeparam>
        /// <param name="f">Func&lt;U,Y&gt; A function to convert U to a Y</param>
        /// <returns></returns>
        public Validation<T, Y> Map<Y>(Func<U, Y> f) {
            if (validation.IsSuccess)
                return Validation.Success(f(getSuccess()));

            return Validation.Failure(validation.ProjectFailure().Get());
        }

        /// <summary>
        /// Flattens and maps through Success side
        /// </summary>
        /// <typeparam name="Y"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Validation<T, Y> FlatMap<Y>(Func<U, Validation<T, Y>> f) {
            if (validation.IsSuccess)
                return f(getSuccess());
            return Validation.Failure(validation.ProjectFailure().Get());
        }

        /// <summary>
        /// Gets the value of the Success or converts the failure to U
        /// </summary>
        /// <param name="or">Function to convert a T to a U</param>
        /// <returns></returns>
        public U GetOrElse(Func<T, U> or) {
            if (validation.IsSuccess)
                return getSuccess();
            return or(validation.ProjectFailure().Get());
        }
    }
}
