namespace Scalesque.Patterns {

    public static class One {
        public static Option<int> unapply(string pattern) {
            return pattern == "one" ? Option.Some(1) : Option.None();
        }
    }

    public static class NinetyNine {
        public static Option<int> unapply(string pattern) {
            return pattern.ToLower() == "ninety nine" ? Option.Some(1) : Option.None();
        }
    }
}
