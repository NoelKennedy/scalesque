namespace Scalesque {

    public abstract partial class Option<T> {
        public static implicit  operator Option<T>(None none) {
            return None<T>.apply();
        }
    }

    /// <summary>
    /// Scalesque use only
    /// </summary>
    public sealed class None {
        internal None() {}
    }
}
