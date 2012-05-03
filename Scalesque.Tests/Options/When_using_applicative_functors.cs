using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Options {
    public class When_using_applicative_functors_on_somes : UnitTestBase{
        private Option<int> t1;
        private Option<int> t2;
        private Option<int> result;

        public override void Given() {
            t1 = 1.ToSome();
            t2 = 1.ToSome();
        }

        public override void Because() {
            result = t1.ApplicativeFunctor(t2, (x, y) => x + y);
        }

        [Test]
        public void It_should_be_some() {
            result.Should().BeOfType<Some<int>>();
        }

        [Test]
        public void It_should_have_mapped_using_the_function() {
            result.Get().Should().Be(2);
        }
    }

    public class When_using_applicative_functors_on_none_and_some : UnitTestBase {
        private Option<int> t1;
        private Option<int> t2;
        private Option<int> result1;
        private Option<int> result2;

        public override void Given() {
            t1 = 1.ToSome();
            t2 = 1.ToNone();
        }

        public override void Because() {
            result1 = t1.ApplicativeFunctor(t2, (x, y) => x + y);
            result2 = t1.ApplicativeFunctor(t2, (x, y) => x + y);
        }

        [Test]
        public void Some_then_none_should_be_none() {
            result1.Should().BeOfType<None<int>>();
        }

        [Test]
        public void None_then_someshould_be_none() {
            result2.Should().BeOfType<None<int>>();
        }
    }
}
