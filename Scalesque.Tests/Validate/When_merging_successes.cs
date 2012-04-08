using System;
using FluentAssertions;
using NUnit.Framework;
using Scalesque.Collections;

namespace Scalesque.Validate
{
    public class When_merging_successes : UnitTestBase {
        private const double FirstSuccessValue = 0.1;
        private const int SecondSuccessValue = 2012;
        private Validation<string, double> check1;
        private Validation<string, int> check2;
        private Validation<NonEmptySList<string>, Tuple<double, int>> mergedChecks;

        public override void Given() {
            check1 = FirstSuccessValue.ToSuccess();
            check2 = SecondSuccessValue.ToSuccess();
        }

        public override void Because() {
            var a = check1.Lift();
            var b = check2.Lift();
            mergedChecks = a.Combine(b);
        }

        [Test]
        public void It_should_result_in_success() {
            mergedChecks.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void It_should_have_accumulated_all_success_values() {
            var successes = mergedChecks.ProjectSuccess().Get();
            successes.Item1.Should().Equals(FirstSuccessValue);
            successes.Item2.Should().Equals(SecondSuccessValue);
        }
    }
}