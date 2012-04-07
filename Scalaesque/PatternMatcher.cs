using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scalesque {
    
    /// <summary>
    /// Allows scala like pattern matching syntax and behaviour
    /// </summary>
    /// <typeparam name="A">The type of the instance you are pattern matching against</typeparam>
    /// <typeparam name="C">The result type of a pattern matching function</typeparam>
    /// <remarks>Pattern matching is a more powerful form of switching.  See http://www.codecommit.com/blog/scala/scala-for-java-refugees-part-4 </remarks>
    public class PatternMatcher<A, C> : IEnumerable<Func<A, Option<C>>> {
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
            list.Add(x=>extractor(x).Map(handler));
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
        /// Adds a predicate which extracts the A to pass to the handler
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="handler"></param>
        public void Add(Predicate<A> pattern, Func<A, C> handler) {
            list.Add(x => pattern(x) ? Option.Some(handler(x)) : Option.None());
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
        public Option<C> Get(A pattern) {
            //This implementation probably isnt the fastest, as it won't short circuit the fold if a pattern matches, but it does demonstrate the power of fp!

            //need to help the compiler a bit here to force conversion of None to None<T>
            return list.FoldLeft<Option<C>,Func<A, Option<C>>>(Option.None(), (acc, maybe) => acc.Or(()=>maybe(pattern)));
        }

        public C GetOrElse(A pattern, Func<C> f) {
            return Get(pattern).GetOrElse(f);
        }

        public IEnumerator<Func<A, Option<C>>> GetEnumerator() {
            //need this to allow the object initialisers in c#.  Not sure why c# compiler requires this.
            //http://blogs.msdn.com/b/madst/archive/2006/10/10/what-is-a-collection_3f00_.aspx
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }


    /// <summary>
    /// A side effect pattern matcher
    /// </summary>
    /// <typeparam name="A">&lt;A&gt; The type of the pattern you want to match against</typeparam>
    public class PatternMatcher<A> : IEnumerable<Func<A, Option<Unit>>> {
        readonly List<Func<A, Option<Unit>>> list;

        public PatternMatcher() {
            list = new List<Func<A, Option<Unit>>>();
        }

        /// <summary>
        /// Pattern matches against the pattern and executes the corresponding side effect if a pattern matches
        /// </summary>
        /// <param name="pattern">A the pattern to match against</param>
        /// <returns>bool  True if a side effect was executed</returns>
        public bool Match(A pattern) {
            return list.Any(f => f(pattern).HasValue);
        }

        /// <summary>
        /// Adds an extractor pattern
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="extractor"></param>
        /// <param name="handler"></param>
        public void Add<B>(Func<A,Option<B>> extractor, Action<B> handler) {
            list.Add(x => extractor(x).Map(b => perform(() => handler(b))));
        }

        /// <summary>
        /// Convenience method for performing actions
        /// </summary>
        private Unit perform(Action action) {
            action();
            return Unit.Value;
        }

        /// <summary>
        /// Adds a predicate pattern matcher
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="handler"></param>
        public void Add(Predicate<A> pattern, Action<A> handler) {
            list.Add(x=>pattern(x) ? Option.Some(perform(()=>handler(x))) : Option.None());
        }

        /// <summary>
        /// Adds an even simpler predicate pattern matcher
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="handler"></param>
        public void Add(Func<bool> pattern, Action handler) {
            list.Add(x => pattern() ? Option.Some(perform(handler)) : Option.None());
        }
        
        public IEnumerator<Func<A, Option<Unit>>> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}

