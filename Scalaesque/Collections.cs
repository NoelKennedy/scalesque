using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Scalesque {

    public static class Collections
    {
        public static A FoldLeft<A, B>(this IEnumerable<B> list, A accumulator, Func<A, B, A> f) {
            return list.Aggregate(accumulator, f);
        }

        public static T Reduce<T>(this IEnumerable<T> list, Func<T,T,T> f)
        {
            var en = list.GetEnumerator();
            en.MoveNext();
            var acc = en.Current;
            while (en.MoveNext())
            {
                acc = f(acc, en.Current);
            }
            return acc;
        }

        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> list, Func<T, U> func)
        {
            foreach (var i in list)
                yield return func(i);
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
        private static IEnumerable<T> EnumerateTail<T>(IEnumerator<T> en)
        {
            while (en.MoveNext()) yield return en.Current;
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) {
            return source.SelectMany(x => x);
        }


        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> f) {
            foreach (var item in enumerable)
                f(item);
        }
    }
}