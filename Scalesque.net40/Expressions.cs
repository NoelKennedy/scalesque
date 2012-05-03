using System;

namespace Scalesque {

    /// <summary>
    /// The start of an If expression
    /// </summary>
    public sealed class IfExpression {
        private readonly bool predicate;

        internal IfExpression(bool predicate) {
            this.predicate = predicate;
        }

        public ElseExpression<T> Then<T>(Func<T> then) {
            return new ElseExpression<T>(predicate, then);
        }
    }

    /// <summary>
    /// An optional else expression
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ElseExpression<T> {
        private readonly bool predicate;
        private readonly Func<T> then;

        internal ElseExpression(bool predicate, Func<T> then) {
            this.predicate = predicate;
            this.then = then;
        }

        /// <summary>
        /// Provides an optional else expression to an if - then expression.  
        /// </summary>
        /// <param name="else"></param>
        /// <returns>T</returns>
        public T Else(Func<T> @else) {
            return predicate ? then() : @else();
        }

        /// <summary>
        /// Not sure about the utility of this but nevermind...
        /// </summary>
        /// <param name="ife"></param>
        /// <returns></returns>
        public static implicit operator Option<T>(ElseExpression<T> ife) {
            if (ife.predicate)
                return ife.then().ToSome();
            else return Option.None();
        }
    }

  
    public static class Expressions {
        /// <summary>
        /// Allows use of if as an expression rather than a statement
        /// </summary>
        /// <param name="x"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <example><code>string result = "Mr T".If(x=>x.Contains("Mr T)).Then(()=>"Rules!").Else(()=>"Fool!");</code></example>
        public static IfExpression If(this object x, bool predicate) {
            return new IfExpression(predicate);
        }

        public static IfExpression If<T>(this T x, Predicate<T> pred) {
            return new IfExpression(pred(x));
        }
    }

   
}
