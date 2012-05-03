using System;
using FluentAssertions;
using NUnit.Framework;
using Scalesque.Collections;

namespace Scalesque.Validate
{
    public abstract class ValidationTestBase : UnitTestBase {
        protected Validation<string, double> check1;
        protected Validation<string, int> check2;
        protected Validation<string, int> check3;
        private const double FirstSuccessValue = 0.1;
        protected const string FailBigTime = "Fail, Big, Time";

        public override void Given() {
            check1 = FirstSuccessValue.ToSuccess();
            check2 = FailBigTime.ToFailure();
            check3 = "Also failed".ToFailure();
        }
    }

    public class When_working_with_some_failures : ValidationTestBase {
        private Validation<NonEmptySList<string>, Tuple<double, int, int>> mergedChecks;

        public override void Because() {
            var a = check1.LiftFailNel();
            var b = check2.LiftFailNel();
            var c = check3.LiftFailNel();
            mergedChecks = a.ApplicativeFunctor(b, c, Tuple.Create);
        }

        [Test]
        public void It_should_result_in_failure() {
            mergedChecks.IsFailure.Should().BeTrue();
        }

        [Test]
        public void It_should_have_accumulated_failure_message() {
            mergedChecks.ProjectFailure().Get().Should().Contain(new[] {FailBigTime, "Also failed"});
        }
    }

    public class When_working_with_only_success : ValidationTestBase {
        private Validation<NonEmptySList<string>, double> results;

        public override void Because() {
            Validation<ISemiJoin<NonEmptySList<string>>, double> a = check1.LiftFailNel();

            results = a.ApplicativeFunctor(a, a, a, (a1, a2, a3, a4) => a1 * a2 * a3 * a4);
        }

        [Test]
        public void It_should_result_in_success() {
            results.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void It_should_have_applied_the_functor() {
            results.ProjectSuccess().Get().Should().BeGreaterOrEqualTo(Math.Pow(0.1, 4));
        }
    }
}