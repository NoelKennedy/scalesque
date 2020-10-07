using System;

namespace Scalesque {

    /// <summary>
    /// My favourite is madrass
    /// </summary>
    public static class YumCurry {

        /// <summary>
        /// Curries a function of arity 2
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func) {
            return a => b => func(a, b);
        }

        /// <summary>
        /// Curries a function of arity 3
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, Func<T2, Func<T3,TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func) {
            return a => b => c =>  func(a, b, c);
        }

        /// <summary>
        /// Curries a function of arity 4
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func) {
            return a => b => c => d => func(a, b, c, d);
        }

        /// <summary>
        /// Curries a function of arity 5
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>> Curry<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func) {
            return a => b => c => d => e =>func(a, b, c, d, e);
        }

        /// <summary>
        /// Curries a function of arity 6
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TResult>>>>>> Curry<T1, T2, T3, T4, T5, T6,TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func) {
            return a => b => c => d => e => f => func(a, b, c, d, e, f);
        }

        /// <summary>
        /// Curries a function of arity 7
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TResult>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
            return a => b => c => d => e => f => g => func(a, b, c, d, e, f, g);
        }

        /// <summary>
        /// Curries a function of arity 8
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, Func<T2,Func<T3,Func<T4,Func<T5,Func<T6,Func<T7,Func<T8, TResult>>>>>>>> Curry<T1,T2,T3,T4,T5,T6,T7,T8,TResult>(this Func<T1,T2,T3,T4,T5,T6,T7,T8,TResult> func) {
            return a => b => c => d => e => f => g => h => func(a, b, c, d, e, f, g, h);
        }
    }
}
