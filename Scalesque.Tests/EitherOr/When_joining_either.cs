using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.EitherOr {

    public class When_joining_either_on_left : UnitTestBase{
        private Either<Either<int, double>, double> eitherEither;
        private Either<int, double> joined;

        public override void Given() {
            Either<string, double> either = "Some ol' blah".ToLeft();
            eitherEither = either.ProjectLeft().Map<Either<int, double>>(str => str.Length.ToLeft());
        }

        public override void Because() {
            joined = eitherEither.JoinLeft();
        }

        [Test]
        public void It_should_be_left() {
            joined.IsLeft.Should().BeTrue();
        }

        [Test]
        public void It_should_have_the_inner_either_left_value() {
            joined.ProjectLeft().Get().Should().Be("Some ol' blah".Length);
        }
    }

    public class When_joining_either_on_right : UnitTestBase {
        private Either<double, Either<double, int>> eitherEither;
        private Either<double, int> joined;

        public override void Given() {
            Either<double, string> either = "Some ol' blah".ToRight();
            eitherEither = either.ProjectRight().Map<Either<double, int>>(str => str.Length.ToRight());
        }

        public override void Because() {
            joined = eitherEither.JoinRight();
        }

        [Test]
        public void It_should_be_right() {
            joined.IsRight.Should().BeTrue();
        }

        [Test]
        public void It_should_have_the_inner_either_right_value() {
            joined.ProjectRight().Get().Should().Be("Some ol' blah".Length);
        }
    }
}
