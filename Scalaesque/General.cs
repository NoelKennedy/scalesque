using System;
using System.Diagnostics;

namespace Scalesque {
    public static class General {

        /// <summary>
        /// Convenience method for String.Format.  The replacement markup is still .net's {0} style.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
// ReSharper disable InconsistentNaming
        //needs to be lower case to avoid collision with static member
        public static string format(this string str, params object[] parameters) {
// ReSharper restore InconsistentNaming

            return String.Format(str, parameters);
        }
    }
}