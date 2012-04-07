

namespace Scalesque {
    using System;

    namespace Scalesque {

        /// <summary>
        /// A projection of the Failure side of an <see cref="Validation{T,U}"/>.  Provides methods for manipulating the potential failure value.
        /// </summary>
        /// <typeparam name="T">T The type of the failure side value</typeparam>
        /// <typeparam name="U">U The type of the Success side value</typeparam>
        public sealed class FailureProjection<T, U> {
            private readonly Validation<T, U> validation;
            private readonly Func<T> getFailure;

            public FailureProjection(Validation<T, U> validation, Func<T> getFailure) {
                this.validation = validation;
                this.getFailure = getFailure;
            }

            /// <summary>
            /// Gets the value of the Failure
            /// </summary>
            /// <exception cref="NotSupportedException">Thrown if called on a Success&lt;T,U&gt;</exception>
            /// <returns></returns>
            public T Get() {
                return getFailure();
            }

            /// <summary>
            /// Gets an optional Failure value
            /// </summary>
            /// <returns>Some&lt;T,U&gt; if Failure, else None&lt;T,U&gt;</returns>
            public Option<T> ToOption() {
                return validation.Fold(failure => Option.Some(failure), Success => Option.None());
            }

            /// <summary>
            /// Performs a side effect on the Failure value
            /// </summary>
            /// <param name="action"></param>
            public void ForEach(Action<T> action) {
                if (validation.IsFailure)
                    action(getFailure());
            }

            /// <summary>
            /// Maps through the Failure side
            /// </summary>
            /// <typeparam name="Y">New type of the Failure</typeparam>
            /// <param name="f">Func&lt;T,Y&gt; A function to convert T to a Y</param>
            /// <returns></returns>
            public Validation<Y, U> Map<Y>(Func<T, Y> f) {
                if (validation.IsFailure)
                    return Validation.Failure(f(getFailure()));

                return Validation.Success(validation.ProjectSuccess().Get());
            }

            /// <summary>
            /// Flattens and maps through Failure side
            /// </summary>
            /// <typeparam name="Y"></typeparam>
            /// <param name="f"></param>
            /// <returns></returns>
            public Validation<Y, U> FlatMap<Y>(Func<T, Validation<Y, U>> f) {
                if (validation.IsFailure)
                    return f(getFailure());

                return Validation.Success(validation.ProjectSuccess().Get());
            }

            /// <summary>
            /// Gets the value of the Failure or converts the Success value to T
            /// </summary>
            /// <param name="or">Function to convert a U to a T</param>
            /// <returns></returns>
            public T GetOrElse(Func<U, T> or) {
                if (validation.IsFailure)
                    return getFailure();
                return or(validation.ProjectSuccess().Get());
            }
        }
    }

}
