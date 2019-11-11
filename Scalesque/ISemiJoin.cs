namespace Scalesque {

    /// <summary>
    /// Identifies semi join categories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISemiJoin<T>{

        T Value { get; }

        /// <summary>
        /// Tell semi join to join with another semi join of the same type
        /// </summary>
        /// <param name="other"></param>
        /// <returns>T a new semi join which joins the two semi-joins</returns>
        T Join(ISemiJoin<T> other);

        /// <summary>
        /// Tell semi join to semi-join with another semi join of the same type
        /// </summary>
        /// <param name="other"></param>
        /// <returns>ISemiJoin&lt;T&gt; a new semi join which joins the two semi-joins</returns>
        ISemiJoin<T> SemiJoin(ISemiJoin<T> other);
    }

}
