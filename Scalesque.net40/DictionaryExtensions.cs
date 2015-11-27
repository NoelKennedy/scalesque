using System;
using System.Collections.Generic;

namespace Scalesque {

    /// <summary>
    /// Provides extension methods for Dictionaries
    /// </summary>
    public static class DictionaryExtensions {

        /// <summary>
        /// Tries to get value from a dictionary, returning an Option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Option<U> Get<T,U>(this IDictionary<T,U> dict, T key) {
            if (dict.ContainsKey(key))
                return Option.Some(dict[key]);
            else
                return Option.None();
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

        /// <summary>
        /// Creates a new dictionary keyed by the original dictionary but maps the values of the original
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Dictionary<TKey, U> MapValues<TKey, T, U>(this Dictionary<TKey, T> dictionary, Func<T, U> f)  {
            return dictionary.FoldLeft(new Dictionary<TKey, U>(), (acc, kv) => {
                acc[kv.Key] = f(kv.Value);
                return acc;
            });
        }
    }
}
