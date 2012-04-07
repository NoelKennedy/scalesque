using System;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.EitherOr
{
    public abstract class When_extracting : UnitTestBase
    {
        protected Either<Exception, string> left;
        protected Either<Exception, string> right;

        public override void Given() {
            left = Either.Left(new Exception("Naughty exception"));
            right = Either.Right("Happy path string");
        }
    }

    public class When_left_extracting : When_extracting{
        private Option<Exception> leftExtracted;
        private Option<Exception> rightExtracted;

        public override void Because() {
            leftExtracted = Left.unapply(left);
            rightExtracted = Left.unapply(right);
        }

        [Test]
        public void Extracting_left_should_be_some() {
            leftExtracted.Should().BeAssignableTo<Some<Exception>>();
        }

        [Test]
        public void Extracting_right_should_be_none(){
            rightExtracted.Should().BeAssignableTo<None<Exception>>();
        }
    }

    public class When_right_extracting : When_extracting {
        private Option<string> leftExtracted;
        private Option<string> rightExtracted;

        public override void Because() {
            leftExtracted = Right.unapply(left);
            rightExtracted = Right.unapply(right);
        }

        [Test]
        public void Extracting_left_should_be_some() {
            leftExtracted.Should().BeAssignableTo<None<string>>();
        }

        [Test]
        public void Extracting_right_should_be_none() {
            rightExtracted.Should().BeAssignableTo<Some<string>>();
        }
    }
}
