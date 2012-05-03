using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Scalesque.Collections;

namespace Scalesque.Validate {

    public class When_merging_failures : UnitTestBase {        
        private Validation<string, double> check1;
        private Validation<string, int> check2;
        private Validation<NonEmptySList<string>, Tuple<double, int>> mergedChecks;

        public override void Given() {
            check1 = "Some problem 2".ToFailure();
            check2 = "Some problem 1".ToFailure();
        }

        public override void Because() {
            var a = check1.LiftFailNel();
            var b = check2.LiftFailNel();
            mergedChecks = a.ApplicativeFunctor(b, Tuple.Create) ;
        }

        [Test]
        public void It_should_result_in_failure() {
            mergedChecks.IsFailure.Should().BeTrue();
        }

        [Test]
        public void It_should_have_accumulated_all_failure_messages() {
            SList<string> failNel = mergedChecks.ProjectFailure().Get();
            failNel.Length.Should().Be(2);
            failNel.Should().Contain("Some problem 1");
            failNel.Should().Contain("Some problem 2");
        }
    }
}
