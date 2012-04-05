using System;
using System.Collections.Generic;

namespace Scalesque {

    public static class Map {

        /// <summary>
        /// Tries to get value from a dictionary, returning an Option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Option<U> Get<T,U>(this IDictionary<T,U> dict, T key) {
            U value;
            dict.TryGetValue(key, out value);
            return Option.apply(value);
        }

        /// <summary>
        /// Gets a value from a dictionary if it exists, or a default if it doesn't
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="orDefault"></param>
        /// <returns></returns>
        public static U GetOrElse<T, U>(this IDictionary<T, U> dict, T key, Func<U> orDefault) {
            return Get(dict, key).GetOrElse(orDefault);
        }
    }
}
