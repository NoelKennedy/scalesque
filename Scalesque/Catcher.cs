using System;

namespace Scalesque {

    /// <summary>
    /// Factory class for Catcher classes
    /// </summary>
    public static class Catcher {
        /// <summary>
        /// Creates a <see cref="Catcher{E1}"/> instance which will catch exceptions of type E1 and handle conversions for you
        /// </summary>
        /// <typeparam name="E1"></typeparam>
        /// <returns></returns>
        public static Catcher<E1> Catching<E1>() where E1:Exception {
            return new Catcher<E1>();
        }
    }

    /// <summary>
    /// Catches exceptions and replaces with Option&lt;T&gt; or Either&lt;E1,T&gt;
    /// </summary>
    /// <typeparam name="E1">E1 the type of the exception you want to catch</typeparam>
    public sealed class Catcher<E1> where E1 : Exception {

        /// <summary>
        /// Tries to execute the function returning an Option&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="f"></param>
        /// <returns>Some&lt;T&gt; if the function doesn't throw an exception of type E1 and None&lt;T&gt; if it does throw throw an exception of type E1</returns>
        public Option<T> ToOption<T>(Func<T> f) {
            try {
                return f().ToSome();
            } catch (E1) {
                return Option.None();
            }
        }

        /// <summary>
        /// Tries to execute the function returning an Either&lt;E1,T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="f"></param>
        /// <returns>Right&lt;E1,T&gt; if the function doesn't throw an exception of type E1 and Left&lt;E1,T&gt; if it does throw throw an exception of type E1</returns>
        public Either<E1, T> ToEither<T>(Func<T> f) {
            try {
                return f().ToRight();
            } catch (E1 e1) {
                return e1.ToLeft();
            }
        }
    }
}
