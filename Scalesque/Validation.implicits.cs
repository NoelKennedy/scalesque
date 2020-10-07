namespace Scalesque {
    public abstract partial class Validation<T,U>
    {

        //this simulates scala's type inference 
        public static implicit operator Validation<T, U>(Failure<T> converted) {
            return new Failure<T, U>(converted.value);
        }

        public static implicit operator Validation<T, U>(Success<U> converted) {
            return new Success<T, U>(converted.value);
        }
    }

    /// <summary>
    /// Scalesque use only
    /// </summary>
    /// <remarks>This class allows us to simulate scala's type inference to return an Validation{T,U} without the libary user having to tell the c# compiler the type of {U}</remarks>
    /// <typeparam name="T"></typeparam>
    public sealed class Failure<T> {
        public readonly T value;

        internal Failure(T value) {
            this.value = value;
        }
    }

    /// <summary>
    /// Scalesque use only
    /// </summary>
    /// <remarks>This class allows us to simulate scala's type inference to return an Validation{T,U} without the libary user having to tell the c# compiler the type of {T}</remarks>
    /// <typeparam name="U"></typeparam>
    public sealed class Success<U> {
        public readonly U value;

        internal Success(U value) {
            this.value = value;
        }
    }
}
