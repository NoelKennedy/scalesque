namespace Scalesque {

    public abstract partial class Either<T,U> {    

        //this simulates scala's type inference 
        public static implicit operator Either<T, U>(Left<T> converted) {
            return new Left<T, U>(converted.value);
        }

        public static implicit operator Either<T, U>(Right<U> converted) {
            return new Right<T, U>(converted.value);
        }
    }

    /// <summary>
    /// Extractor for Left{T}
    /// </summary>
    /// <remarks>This class allows us to simulate scala's type inference to return an Either{T,U} without the libary user having to tell the c# compiler the type of {U}</remarks>
    /// <typeparam name="T"></typeparam>
    public sealed class Left<T> {
        public readonly T value;

        internal Left(T value) {
            this.value = value;
        }

        /// <summary>
        /// Extractor for Left{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="either"></param>
        /// <returns></returns>
        public static Option<T> unapply<U>(Either<T,U> either) {
            return Option.apply(either as Left<T, U>).Map(x=>x.Get());
        }
    }

    /// <summary>
    /// Extractor for Right{T}
    /// </summary>
    /// <remarks>This class allows us to simulate scala's type inference to return an Either{T,U} without the libary user having to tell the c# compiler the type of {T}</remarks>
    /// <typeparam name="U"></typeparam>
    public sealed class Right<U> {
        public readonly U value;

        internal Right(U value) {
            this.value = value;
        }

        /// <summary>
        /// Extractor for Right{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="either"></param>
        /// <returns></returns>
        public static Option<U> unapply<T>(Either<T, U> either) {
            return Option.apply(either as Right<T, U>).Map(x => x.Get());
        }
    }
}
