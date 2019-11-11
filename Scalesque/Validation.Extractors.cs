using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scalesque {

    /// <summary>
    /// An extractor class for Validations
    /// </summary>
    public static class Success {
        /// <summary>
        /// An extractor for the success side of a Validation&lt;T,U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="validation"></param>
        /// <returns></returns>
        public static Option<U> unapply<T,U>(Validation<T,U> validation) {
            return validation.ProjectSuccess().ToOption();
        }
    }

    /// <summary>
    /// An extractor class for Validations
    /// </summary>
    public static class Failure {
        /// <summary>
        /// An extractor for the failure side of a Validation&ltT,U&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="validation"></param>
        /// <returns></returns>
        public static Option<T> unapply<T, U>(Validation<T, U> validation) {
            return validation.ProjectFailure().ToOption();
        }
    }
}
