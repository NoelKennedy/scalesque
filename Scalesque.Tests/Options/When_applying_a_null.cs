using System;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Optional
{
    public class When_applying_a_null : UnitTestBase
    {
        private Option<string> option;

        public override void Because()
        {
            string blah = null;
            option = Option.apply(blah);
        }

        [Test]
        public void It_should_be_none() {
            option.Should().BeAssignableTo<None<string>>();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentNullException))]
        public void It_should_throw_on_get() {
            option.Get();
        }

        [Test]
        public void It_should_map_to_a_new_type() {
            option.Map(x => x.Length).Should().BeAssignableTo<None<int>>();
        }

        [Test]
        public void It_should_return_the_else() {
            option.GetOrElse(() => "mee!").Should().Be("mee!");
        }

        [Test]
        public void It_should_return_the_other() {
            option.FlatMap<int>(x => Option.apply(1)).Should<int>().BeAssignableTo<None<int>>();
        }

    }
}
