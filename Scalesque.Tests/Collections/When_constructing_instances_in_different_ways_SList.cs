using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Collections {

    public class When_constructing_instances_in_different_ways_SList:UnitTestBase {
        private SList<string> list;
        private SList<string> list2;

        public override void Because() {
            list = Nil<string>.Instance + "blah" + "Jubbins" +  "Your more general gubbins";
            list2 = SList.apply("Your more general gubbins", "Jubbins", "blah");
        }

        [Test]
        public void It_should_create_equal_length_lists() {
            list.Length.Should().Be(3);
            list2.Length.Should().Be(list.Length);
        }

        [Test]
        public void It_should_result_in_lists_with_matching_heads() {
            list.Head.Should().Be(list2.Head);
        }

        [Test]
        public void It_should_result_in_lists_matching_tails() {
            list.Tail.Should().Equal(list2.Tail);
        }
    }
}
