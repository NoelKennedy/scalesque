using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Collections {
    public class When_working_with_Nil_of_different_types :UnitTestBase {
        private Nil<string> nilString;
        private Nil<UnitTestBase> nilElse;

        public override void Because() {
            nilString = Nil<string>.Instance;
            nilElse = Nil<UnitTestBase>.Instance;
        }

        [Test]
        public void Nils_of_different_types_should_be_equal() {
            nilString.Equals(nilElse).Should().BeTrue();
        }
    }
}
