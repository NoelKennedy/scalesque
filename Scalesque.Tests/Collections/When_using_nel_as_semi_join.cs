using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Collections {
    public class When_using_nel_as_semi_join  :UnitTestBase {
        private ISemiJoin<NonEmptySList<int>> nel1;
        private ISemiJoin<NonEmptySList<int>> nel2;
        private ISemiJoin<NonEmptySList<int>> joined;

        public override void Given() {
            nel1 = 1.cons(2.cons(Nil.apply()));
            nel2 = 3.cons(4.cons(Nil.apply()));
        }

        public override void Because() {
            joined = nel2.Join(nel1);
        }

        [Test]
        public void It_should_have_joined_the_contents() {
            joined.Value.Should().ContainInOrder(new[] {2,1,3,4});

        }
    }
}
