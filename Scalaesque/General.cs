using System;
using System.Diagnostics;

namespace Scalesque {
    public static class General {

        [DebuggerStepThrough]
// ReSharper disable InconsistentNaming
            //needs to be lower case to avoid collision with static member
        public static string format(this string str, params object[] parameters) {
// ReSharper restore InconsistentNaming
            return String.Format(str, parameters);
        }
    }
}