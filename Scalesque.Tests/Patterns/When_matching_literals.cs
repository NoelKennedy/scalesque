using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Patterns {

    public class When_matching_literals :UnitTestBase{
        private PatternMatcher<string, string> matcher;
        private Option<string> result;

        public override void Given() {
            matcher = new PatternMatcher<string, string> {{"Hello", () => "World"}};
        }

        public override void Because() {
            result = matcher.Get("Hello");
        }

        [Test]
        public void It_should_have_matched_the_literal_pattern() {
            result.Get().Should().Be("World");
        }
    }
}
