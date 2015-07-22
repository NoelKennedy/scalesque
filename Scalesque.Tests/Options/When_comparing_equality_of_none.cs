using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Options
{
    [TestFixture]
    public class When_comparing_equality_of_none : UnitTestBase
    {

        private Option<string> option1;
        private Option<string> option2;
        private Option<string> option3;
        private Option<int> option4;
        private Option<int> option5;

        public override void Because() {
            option1 = Option.apply<string>(null);
            option2 = Option.apply<string>(null);
            option3 = Option.apply<string>("some non-null string");
            option4 = Option.None();
            option5 = Option.apply(1);
        }

        [Test]
        public void It_should_equal_itself() {
            option1.Equals(option1).Should().BeTrue();
        }

        [Test]
        public void It_should_be_equal_to_another_none() {
            option1.Equals(option2).Should().BeTrue();
        }

        [Test]
        public void It_should_not_be_equal_to_a_some_of_the_same_type() {
            option1.Equals(option3).Should().BeFalse();
        }

        [Test]
        public void It_should_not_be_equal_to_a_none_of_a_different_type() {
            option1.Equals(option4).Should().BeFalse(); //avoids test framework comparing them as collections
        }

        [Test]
        public void It_should_not_be_equal_to_a_some_of_a_different_type() {
            option1.Equals(option5).Should().BeFalse();
        }

        [Test]
        public void It_should_not_be_equal_to_a_some_of_a_different_typexx() {
            bool? nullableValueType = null;
            Option.apply(nullableValueType).Should().Equal(Option.apply(nullableValueType));
        }
    }
}