namespace Scalesque {

#if NoFrameworkTuples

    /// <summary>
    /// Provides tuples in versions of the .net Framework that don't have them in the base library
    /// </summary>
    public static class Tuple {
        public static Tuple<T1> Create<T1>(T1 item1) {
            return new Tuple<T1>(item1);
        }
        
        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) {
            return new Tuple<T1, T2>(item1, item2);
        }

        public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) {
            return new Tuple<T1, T2, T3>(item1, item2, item3);
        }

        public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) {
            return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }

        public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) {
            return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) {
            return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4,item5,item6,item7);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(item1, item2, item3, item4, item5, item6, item7, Create(item8));
        }
    }

    public sealed class Tuple<T1> {
        public readonly T1 Item1;

        public Tuple(T1 item1) {
            Item1 = item1;
        }

        public override bool Equals(object obj) {
            var other = obj as Tuple<T1>;

            if (other != null)
                return Item1.Equals(other.Item1);
            return false;
        }

        public override string ToString(){
            return "({0})".format(Item1);
        }
    }

    public sealed class Tuple<T1, T2> {
        public readonly T1 Item1;
        public readonly T2 Item2;

        public Tuple(T1 item1, T2 item2) {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Tuple<T1,T2>;

            if (other != null)
                return Item1.Equals(other.Item1) && Item2.Equals(other.Item2);
            return false;
        }

        public override string ToString()
        {
            return "({0}, {1})".format(Item1, Item2);
        }
    }

    public sealed class Tuple<T1, T2, T3> {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;

        public Tuple(T1 item1, T2 item2, T3 item3) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        public override bool Equals(object obj) {
            var other = obj as Tuple<T1,T2,T3>;

            if (other != null)
                return Item1.Equals(other.Item1) && Item2.Equals(other.Item2) && Item3.Equals(other.Item3);
            return false;
        }

        public override string ToString()
        {
            return "({0}, {1}, {2})".format(Item1, Item2, Item3);
        }
    }

    public sealed class Tuple<T1, T2, T3, T4> {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;

        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Tuple<T1,T2,T3,T4>;

            if (other != null)
                return Item1.Equals(other.Item1) && Item2.Equals(other.Item2) && Item3.Equals(other.Item3) && Item4.Equals(other.Item4);
            return false;
        }

        public override string ToString()
        {
            return "({0}, {1}, {2}, {3})".format(Item1, Item2, Item3, Item4);
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5> {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;
        public readonly T5 Item5;

        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
        }

        public override string ToString()
        {
            return "({0}, {1}, {2}, {3}, {4})".format(Item1, Item2, Item3, Item4, Item5);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Tuple<T1, T2, T3, T4, T5>;

            if (other != null)
                return Item1.Equals(other.Item1) && Item2.Equals(other.Item2) && Item3.Equals(other.Item3) && Item4.Equals(other.Item4) && Item5.Equals(other.Item5);
            return false;
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, T6> {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;
        public readonly T5 Item5;
        public readonly T6 Item6;

        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Tuple<T1, T2, T3, T4, T5, T6>;

            if (other != null)
                return Item1.Equals(other.Item1) && Item2.Equals(other.Item2) && Item3.Equals(other.Item3) && Item4.Equals(other.Item4) && Item5.Equals(other.Item5) && Item6.Equals(other.Item6);;
            return false;
        }

        public override string ToString()
        {
            return "({0}, {1}, {2}, {3}, {4}, {5})".format(Item1, Item2, Item3, Item4, Item5, Item6);
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7> {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;
        public readonly T5 Item5;
        public readonly T6 Item6;
        public readonly T7 Item7;

        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Tuple<T1, T2, T3, T4, T5, T6, T7>;

            if (other != null)
                return Item1.Equals(other.Item1) && Item2.Equals(other.Item2) && Item3.Equals(other.Item3) && Item4.Equals(other.Item4) && Item5.Equals(other.Item5) && Item6.Equals(other.Item6) && Item7.Equals(other.Item7);
            return false;
        }

        public override string ToString()
        {
            return "({0}, {1}, {2}, {3}, {4}, {5}, {6})".format(Item1, Item2, Item3, Item4, Item5, Item6);
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8> {
        public readonly T1 Item1;
        public readonly T2 Item2;
        public readonly T3 Item3;
        public readonly T4 Item4;
        public readonly T5 Item5;
        public readonly T6 Item6;
        public readonly T7 Item7;
        public readonly T8 Item8;

        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Tuple<T1, T2, T3, T4, T5, T6, T7, T8>;

            if (other != null)
                return Item1.Equals(other.Item1) && Item2.Equals(other.Item2) && Item3.Equals(other.Item3) && Item4.Equals(other.Item4) && Item5.Equals(other.Item5) && Item6.Equals(other.Item6) && Item7.Equals(other.Item7) && Item8.Equals(other.Item8);
            return false;
        }

        public override string ToString()
        {
            return "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})".format(Item1, Item2, Item3, Item4, Item5, Item6, Item8);
        }
    }

#endif
}
