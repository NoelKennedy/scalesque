using System;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.FunctionApplication {
    
    public class When_creating_partially_applied_functions : UnitTestBase {
        private Func<int, int, int, int, int, int, int, int> func;
        private Func<int, int> apply6Params;

        public override void Given() {
            func = (v1, v2, v3, v4, v5, v6, v7) => v1 + v2 + v3 + v4 + v5 + v6 + v7;
        }

        public override void Because() {
            apply6Params = func.Partial(1, 1, 1, 1, 1, 1);
        }

        [Test]
        public void It_should_have_applied_the_first_six_parameters_creating_a_function_of_arity_1() {
            apply6Params(1).Should().Be(7); 
        }
    }
}
