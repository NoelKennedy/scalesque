using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Patterns {
    public abstract class When_pattern_matching_option : UnitTestBase {
        protected PatternMatcher<Option<string>, int> matcher;

        public override void Given() {
            matcher = new PatternMatcher<Option<string>, int>() 
                      {
                          {Some.unapply, str => str.Length}, //matches Some<string>
                          {None.unapply, () => 1977}   //matches None<string>
                      };
        }
    }

    public class When_matching_some:When_pattern_matching_option
    {
        private Option<int> result;

        public override void Because() {
            result = matcher.Get(Option.apply("Should match some"));
        }

        [Test]
        public void It_should_have_matched_the_some_extractor() {
            result.Get().Should().Be("Should match some".Length);
        }
    }

    public class When_matching_none:When_pattern_matching_option
    {
        private Option<int> result;

        public override void Because() {
            result = matcher.Get(Option.None());
        }

        [Test]
        public void It_should_have_matched_the_none_extractor() {
            result.Get().Should().Be(1977);
        }
    }
}
