using System;
using System.Collections;
using System.Collections.Generic;

namespace Scalesque {
    
    /// <summary>
    /// Allows scala like pattern matching syntax and behaviour
    /// </summary>
    /// <typeparam name="A">The type of the instance you are pattern matching against</typeparam>
    /// <typeparam name="C">The result type of a pattern matching function</typeparam>
    /// <remarks>Pattern matching is a more powerful form of switching.  See http://www.codecommit.com/blog/scala/scala-for-java-refugees-part-4 </remarks>
    public class PatternMatcher<A,C> : IEnumerable<A> {
        private readonly List<Func<A, Option<C>>> list;

        public PatternMatcher() {
            list = new List<Func<A, Option<C>>>();
        }

        /// <summary>
        /// Adds an extractor to match on the pattern, and a handler to invoke if the extractor succeeds
        /// </summary>
        /// <typeparam name="B">{B} The return type of an extraction</typeparam>
        /// <param name="extractor">Func{A, Option{B}} Function which tries to extract a {B} from the {A}</param>
        /// <param name="handler">Func{B,C} Handler function which takes the successfully extracted {B} from the extractor and converts it to a {C}</param>
        public void Add<B>(Func<A, Option<B>> extractor, Func<B,C> handler) {
            list.Add(x=>extractor(x).map(handler));
        }

        /// <summary>
        /// Allows the adding of a default matching pattern.  This matches all patterns.
        /// </summary>
        /// <param name="f"></param>
        public void Add(Func<C> f) {
            list.Add(x => Option.apply(f()));
        }

        /// <summary>
        /// Adds a predicate pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="handler"></param>
        public void Add(Predicate<A> pattern, Func<C> handler) {
            list.Add(x=>pattern(x) ? Option.Some(handler()) : Option.None());
        }

        /// <summary>
        /// Adds a even simpler predicate pattern
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="handler"></param>
        public void Add(Func<bool> predicate, Func<C> handler) {
            Add(x=>predicate(), handler);
        }

        /// <summary>
        /// Checks the pattern against the pattern matchers.  The first extractor which matches the pattern is the result.  If no patterns match, the result is None{T}
        /// </summary>
        /// <param name="pattern">A</param>
        /// <returns>Option{C}.  Some{C} if a pattern matches, else None{C}</returns>
        public Option<C> get(A pattern) {
            //This implementation probably isnt the fastest, as it won't short circuit the fold if a pattern matches, but it does demonstrate the power of fp!

            //need to help the compiler a bit here to force conversion of None to None<T>
            return list.FoldLeft<Option<C>,Func<A, Option<C>>>(Option.None(), (acc, maybe) => acc.or(()=>maybe(pattern)));
        }

        public C getOrElse(A pattern, Func<C> f) {
            return get(pattern).getOrElse(f);
        }

        public IEnumerator<A> GetEnumerator() {
            //need this to allow the object initialisers in c#.  Not sure why c# compiler requires this.
            //http://blogs.msdn.com/b/madst/archive/2006/10/10/what-is-a-collection_3f00_.aspx
            throw new NotImplementedException(); 
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
