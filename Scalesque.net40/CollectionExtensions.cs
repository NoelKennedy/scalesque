using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Scalesque {

    public static class CollectionExtensions
    {
        /// <summary>
        /// Folds a collection of instances of type &lt;T&gt; into a single instance of type &lt;U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="list"></param>
        /// <param name="accumulator"></param>
        /// <param name="f"></param>
        /// <returns>&lt;U&gt;</returns>
        public static U FoldLeft<T, U>(this IEnumerable<T> list, U accumulator, Func<U, T, U> f) {
            return list.Aggregate(accumulator, f);
        }

        public static T Reduce<T>(this IEnumerable<T> list, Func<T,T,T> f){
            var en = list.GetEnumerator();
            en.MoveNext();
            var acc = en.Current;
            while (en.MoveNext())
            {
                acc = f(acc, en.Current);
            }
            return acc;
        }

        /// <summary>
        /// Maps a type &lt;T&gt; to a &lt;U&gt;  Alias for Linq Select
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns>&lt;U&gt;</returns>
        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> list, Func<T, U> func) {
            return list.Select(func);
        }

        /// <summary>
        /// Maps source to Option T then flattens
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="source"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static IEnumerable<T> FlatMap<T, U>(this IEnumerable<U> source, Func<U, Option<T>> f) => source.Map(f).Flatten();


        /// <summary>
        /// Gets the head of a IEnumerable wrapped in an Option monad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Option<T> HeadOption<T>(this IEnumerable<T> source)
        {
            IList<T> list = source as IList<T>;
            if (list != null)
            {
                if (list.Count > 0) return list[0].ToSome();
            }
            else
            {
                using (IEnumerator<T> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext()) return enumerator.Current.ToSome();
                }
            }
            return Option.None();
        }

        [DebuggerStepThrough]
        public static Tuple<T, IEnumerable<T>> HeadAndTail<T>(this IEnumerable<T> source)
        {
            // Get first element of the 'source' (assuming it is there)
            var en = source.GetEnumerator();
            en.MoveNext();
            // Return first element and Enumerable that iterates over the rest
            return Tuple.Create(en.Current, EnumerateTail(en));
        }

        // Turn remaining (unconsumed) elements of enumerator into enumerable
        private static IEnumerable<T> EnumerateTail<T>(IEnumerator<T> en) {
            while (en.MoveNext()) yield return en.Current;
        }


#if Covariant
        /// <summary>
        /// Flattens an IEnumerable&lt;IEnumerable&lt;T&gt;&gt; into IEnumerable&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks>Requires a version of c# that has covariance for generic types c# >= 4.0</remarks>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) {
            return source.SelectMany(x => x);
        }
#endif


        /// <summary>
        /// Performs a side effect on each member of a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="f"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> f) {
            foreach (var item in enumerable)
                f(item);
        }
    }
}