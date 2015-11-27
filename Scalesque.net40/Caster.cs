using System;

namespace Scalesque
{
    /// <summary>
    ///     Functions to make casting more friendly
    /// </summary>
    public static class Caster
    {

        /// <summary>
        /// Converts an optional value type to the .net nullable type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T? ToNullableValueType<T>(this Option<T> option) where T : struct
        {
            return option.HasValue ? option.Get() : new T?();
        }

        /// <summary>
        /// Converts an optional class type to a .net nullable reference
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T ToNullable<T>(this Option<T> option) where T : class
        {
            return option.HasValue ? option.Get() : null;
        }

        /// <summary>
        ///     Casts an object reference to another type and wraps the result in Option
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>Some if the object can be successfully cast to T else None</returns>
        public static Option<T> To<T>(this object obj) where T : class
        {
            var tryCase = obj as T;
            if (tryCase == null) return Option.None();
            return tryCase.ToSome();
        }

        /// <summary>
        ///     Attempts to convert a string to a Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Option<Guid> ToGuid(this string str)
        {
            Guid guid;
            if (Guid.TryParse(str, out guid)) return guid.ToSome();
            return Option.None();
        }

        /// <summary>
        ///     Attempts to convert a string to a int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Option<int> ToInt(this string str)
        {
            int result;
            if (int.TryParse(str, out result)) return result.ToSome();
            else return Option.None();
        }

        /// <summary>
        ///     Attempts to convert a string to a Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Option<long> ToLong(this string str)
        {
            long result;
            if (Int64.TryParse(str, out result)) return result.ToSome();
            return Option.None();
        }

        /// <summary>
        /// Converts an Option&lt;T&gt; from a nullable value type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Option<T> ToOption<T>(this T? source) where T : struct
        {
            return source.HasValue ? Option.apply(source.Value) : Option.None();
        }

        /// <summary>
        /// Maps a nullable type to an Option type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Option<T> ToOption<T>(this T source) where T : class
        {
            return Option.apply(source);
        }
    }
}
