using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Dictionary {
    public abstract class When_getting_items_from_dictionary_base : UnitTestBase {
        protected Dictionary<string, int> dictionary;

        public override void Given() {
            base.Given();

            dictionary = new Dictionary<string, int> {{"hello", 1}};
        }
    }

    public class When_item_is_not_in_dictionary : When_getting_items_from_dictionary_base
    {
        private int result;

        public override void Because() {
            result = dictionary.GetOrElse("key not in dictionary", () => 2);
        }

        [Test]
        public void It_should_return_the_lamba_default_rather_than_the_value_type_default() {
            result.Should().Be(2);
        }
    }

    public class When_item_is_in_dictionary : When_getting_items_from_dictionary_base {
        private int result;

        public override void Because() {
            result = dictionary.GetOrElse("hello", () => 2);
        }

        [Test]
        public void It_should_return_the_lamba_default_rather_than_the_value_type_default() {
            result.Should().Be(1);
        }
    }
}
