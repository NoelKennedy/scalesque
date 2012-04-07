namespace Scalesque {
    public static class Some{
        public static Option<T> unapply<T>(Option<T> option) {
            return option;
        }
    }

    /// <summary>
    /// Extractor for None. Instance use, scalesque only (improces c# compiler type inference)
    /// </summary>
    public sealed class None {
        internal None() { }

        public static bool unapply<T>(Option<T> option) {
            return !option.HasValue;
        }
    }
}
