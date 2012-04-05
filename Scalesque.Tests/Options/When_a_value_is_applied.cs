using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Options {

    [TestFixture]
    public class When_a_value_is_applied : UnitTestBase {

        const string text = "test";

        private Option<string> option;
        
        public override void Because() {
            option = Option.apply(text);
        }

        [Test]
        public void It_should_be_some() {
            option.Should().BeAssignableTo<Some<string>>();
        }

        [Test]
        public void It_should_have_a_value() {
            option.HasValue.Should().BeTrue();
        }

        [Test]
        public void It_should_not_be_empty() {
            option.IsEmpty.Should().BeFalse();
        }

        [Test]
        public void It_should_get_the_applied_value() {
            option.Get().Should().Be(text);
        }

        [Test]
        public void It_should_enumerate_the_applied_value() {
            option.Single().Should().Be(text);
        }

        [Test]
        public void It_should_map_the_applied_value() {
            option.Map(x => x.Length).Get().Should().Be(text.Length);
        }

        [Test]
        public void It_should_get_the_original_not_alternate() {
            option.GetOrElse(() => "not me guv!").Should().Be(text);
        }

        [Test]
        public void It_should_return_the_other() {
            option.FlatMap(x => Option.apply("mee!")).Get().Should().Be("mee!");
        }
    }
}
