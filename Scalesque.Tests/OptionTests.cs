using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Scalesque {
    
    [TestFixture]
    public abstract class UnitTestBase {

        [TestFixtureSetUp]
        public void PrepareFixture() {
            Given();
            Becuase();
        }

        public virtual void Given() {}

        public abstract void Becuase();



    }
    [TestFixture]
    public class OptionTests : UnitTestBase {
        const string text = "test";
        private Option<string> option;
        
        public override void Becuase() {
            option = Option.apply(text);
        }

        [Test]
        public void Applying_non_null_reference_should_be_some() {
            option.Should().BeAssignableTo<Some<string>>();
        }

        
        

        
    }
}
