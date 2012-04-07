using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Validate {

    public class When_accumulating_failures : UnitTestBase {
        private Validation<string, int> check1;
        private Validation<string, int> check2;
        private Validation<NonEmptyList<string>, int> mergedChecks;

        public override void Given() {
            check1 = "Some problem 1".ToFailure();
            check2 = "Some problem 2".ToFailure();
        }

        public override void Because() {
            mergedChecks = check1.LiftFailNel().MergeFailure(check2.LiftFailNel());
        }

        [Test]
        public void It_should_have_accumulated_both_failure_messages() {
            NonEmptyList<string> failNel = mergedChecks.ProjectFailure().Get();
            failNel.Should().Contain("Some problem 1");
            failNel.Should().Contain("Some problem 2");
        }
    }
}
