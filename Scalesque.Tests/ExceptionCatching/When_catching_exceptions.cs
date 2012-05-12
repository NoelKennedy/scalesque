using System;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.ExceptionCatching {

    public class When_catching_exceptions :UnitTestBase{
        private string myVariable;
        private Option<int> mbResult1;
        private Either<NullReferenceException, int> mbResult2;

        public override void Given() {
            myVariable = null;
        }

        public override void Because() {
            mbResult1 = Catcher.Catching<NullReferenceException>().ToOption(() => myVariable.Length);
            mbResult2 = Catcher.Catching<NullReferenceException>().ToEither(() => myVariable.Length);
        }

        [Test]
        public void It_should_have_caught_and_converted_to_none() {
            mbResult1.Should().BeOfType<None<int>>();
        }

        [Test]
        public void It_should_have_caught_and_converted_to_left() {
            mbResult2.Should().BeOfType<Left<NullReferenceException, int>>();
        }

        [Test]
        public void It_should_have_caught_and_left_should_contain_the_exception() {
            mbResult2.ProjectLeft().Get().Should().BeOfType<NullReferenceException>();
        }
    }

    public class When_exceptions_arent_thrown : UnitTestBase
    {
        private string myVariable;
        private Option<int> mbResult1;
        private Either<NullReferenceException, int> mbResult2;

        public override void Given() {
            myVariable = "I'm not null this time";
        }

        public override void Because() {
            mbResult1 = Catcher.Catching<NullReferenceException>().ToOption(() => myVariable.Length);
            mbResult2 = Catcher.Catching<NullReferenceException>().ToEither(() => myVariable.Length);
        }

        [Test]
        public void It_should_be_a_some_of_the_function_return_value() {
            mbResult1.Should().BeOfType<Some<int>>();
            mbResult1.Get().Should().Be(myVariable.Length);
        }

        [Test]
        public void It_should_be_right_of_the_function_return_value() {
            mbResult2.Should().BeOfType<Right<NullReferenceException, int>>();
            mbResult2.ProjectRight().Get().Should().Be(myVariable.Length);
        }

    }
}
