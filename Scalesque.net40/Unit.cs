namespace Scalesque {

    /// <summary>
    /// Represents a method with side effects
    /// </summary>
    public class Unit {
        public static Unit Value { get; private set; }
        private Unit() {}
        static Unit() {
            Value = new Unit();
        }
    }
}
