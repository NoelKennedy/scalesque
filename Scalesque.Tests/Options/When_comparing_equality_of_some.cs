using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Options
{
    [TestFixture]
    public class When_comparing_equality_of_some : UnitTestBase
    {

        private Option<string> option1;
        private Option<string> option2;
        private Option<string> option3;
        private Option<int> option4;
        private Option<int> option5;
        private Option<string> option6;

        public override void Because() {
            option1 = Option.apply("some non-null string");
            option2 = Option.apply("some non-null string");
            option3 = Option.None();
            option4 = Option.apply(1);
            option5 = Option.None();
            option6 = Option.apply("a different string");
        }

        [Test]
        public void It_should_equal_itself() {
            option1.Should().Equal(option1);
        }

        [Test]
        public void It_should_be_equal_to_another_instance_with_the_same_value() {
            option1.Should().Equal(option2);
        }

        [Test]
        public void It_should_not_be_equal_to_a_some_of_the_same_type_but_a_different_value() {
            option1.Should().NotEqual(option6);
        }

        [Test]
        public void It_should_not_be_equal_to_a_none_of_the_same_type() {
            option1.Should().NotEqual(option3);
        }

        [Test]
        public void It_should_not_be_equal_to_a_none_of_a_different_type() {
            option1.Should().NotEqual(option4);
        }

        [Test]
        public void It_should_not_be_equal_to_a_some_of_a_different_type() {
            option1.Should().NotEqual(option5);
        }
    }
}