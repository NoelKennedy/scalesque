namespace Scalesque
{
    /// <summary>
    /// Left extractor
    /// </summary>
    public static class Left {

        /// <summary>
        /// Extractor for Left{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="either"></param>
        /// <returns></returns>
        public static Option<T> unapply<T, U>(Either<T, U> either) {
            return Option.apply(either as Left<T, U>).Map(x => x.Get());
        }
    }

    /// <summary>
    /// Extractor for Right
    /// </summary>
    public static class Right {

        /// <summary>
        /// Extractor for Right{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="either"></param>
        /// <returns></returns>
        public static Option<U> unapply<T,U>(Either<T, U> either) {
            return Option.apply(either as Right<T, U>).Map(x => x.Get());
        }
    }
   
}
