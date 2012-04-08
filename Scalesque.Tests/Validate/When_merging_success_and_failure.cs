using System;
using FluentAssertions;
using NUnit.Framework;
using Scalesque.Collections;

namespace Scalesque.Validate
{
    public class When_merging_success_and_failure : UnitTestBase {
        private const double FirstSuccessValue = 0.1;
        private const string FailBigTime = "Fail, Big, Time";

        private Validation<string, double> check1;
        private Validation<string, int> check2;
        private Validation<NonEmptySList<string>, Tuple<double, int>> mergedChecks;

        public override void Given() {
            check1 = FirstSuccessValue.ToSuccess();
            check2 = FailBigTime.ToFailure();
        }

        public override void Because() {
            var a = check1.Lift();
            var b = check2.Lift();
            mergedChecks = a.Combine(b);
        }

        [Test]
        public void It_should_result_in_failure() {
            mergedChecks.IsFailure.Should().BeTrue();
        }

        [Test]
        public void It_should_have_accumulated_failure_message() {
            mergedChecks.ProjectFailure().Get().Head.Should().Be(FailBigTime);
        }
    }
}