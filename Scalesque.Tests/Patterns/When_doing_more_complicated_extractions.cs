using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque.Patterns {

    class Foo {
        public bool IsOrderOfBritishEmpire { get; set; }
        public bool IsSidney { get; set; }
    }
    class Bar {
        public string Title { get; set; }
        public string Name { get; set; }
    }

    class Mar {
        public int SomeNumber { get; set; }
    }

    class When_doing_more_complicated_extractions : UnitTestBase {
        private Foo fooSir;
        private Foo fooPoitier;
        private PatternMatcher<Foo, Mar> matcher;
        private Option<Mar> sir;
        private Option<Mar> mr;


        private Option<Bar> KnightedFoosToBar(Foo foo) {
            return foo.IsOrderOfBritishEmpire ? Option.Some(new Bar {Title = "Sir", Name = "BigHead"}) : Option.None();
        }

        private Option<Bar> MrTibbs(Foo foo) {
            return foo.IsSidney ? Option.Some(new Bar { Title = "Mr", Name = "Tibbs" }) : Option.None();
        }

        public override void Given() {
            fooSir = new Foo { IsOrderOfBritishEmpire = true };
            fooPoitier = new Foo { IsSidney = true};

            matcher = new PatternMatcher<Foo, Mar> {
                {KnightedFoosToBar, (Bar bar) => new Mar {SomeNumber = 999}},
                {MrTibbs, (Bar bar) => new Mar {SomeNumber = bar.Name.Length}},
                {()=>new Mar{SomeNumber = 0}} //alternate to GetOrElse for default
            };
        }

        public override void Because() {
            sir = matcher.Get(fooSir);
            mr = matcher.Get(fooPoitier);
        }

        [Test]
        public void It_should_match_sir() {
            sir.Get().SomeNumber.Should().Be(999);
        }

        [Test]
        public void It_should_match_tibs() {
            mr.Get().SomeNumber.Should().Be("tibbs".Length);
        }
        [Test]
        public void It_should_have_default_pattern() {
            matcher.Get(new Foo()).Get().SomeNumber.Should().Be(0);
        }

    }
}
