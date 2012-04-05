using System;
using System.Diagnostics;

namespace Scalesque {
    public static class General {

        [DebuggerStepThrough]
        public static string format(this string str, params object[] parameters) {
            return String.Format(str, parameters);
        }
    }
}