using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Patterns {

    public class When_doing_simple_pattern_matching : UnitTestBase {
        private PatternMatcher<string, int> matcher;
        private Option<int> shouldMatch;
        private Option<int> noMatch;

        public override void Given() {
            matcher = new PatternMatcher<string, int> {
                {One.unapply, x => x},
                {NinetyNine.unapply, x => x}
            };
        }

        public override void Because() {
            shouldMatch = matcher.Get("one");
            noMatch = matcher.Get("My dog");
        }

        [Test]
        public void It_should_match_one() {
            shouldMatch.Should().BeAssignableTo<Some<int>>();
            shouldMatch.Get().Should().Be(1);
        }

        [Test]
        public void It_should_not_match_my_dog() {
            noMatch.Should().BeAssignableTo<None<int>>();
        }

        [Test]
        public void It_should_return_alternate_if_pattern_is_not_matched() {
            matcher.GetOrElse("foo", () => 1972).Should().Be(1972);
        }
    }
}
