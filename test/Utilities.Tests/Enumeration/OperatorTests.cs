using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Utilities.Examples.Enummeration;
using Xunit;
using static Utilities.Enummeration;

namespace Utilities.Tests.Enumeration
{
    public class OperatorTests
    {

        [Fact]
        public void CanConvertToIntAndString()
        {
            ((int) BitFlagSample.UnexpectedError).Should().Be(-1);
            ((string) BitFlagSample.UnexpectedError).Should().Be("Unexpected Error has occurred");
            BitFlagSample.UnexpectedError.ToString().Should().Be("Unexpected Error has occurred");
        }

        [Fact]
        public void CanConvertFromIntAndString()
        {
            ((BitFlagSample) 0).Should().Be(BitFlagSample.Ok);
            ((BitFlagSample) "OK").Should().Be(BitFlagSample.Ok);
        }

        [Fact]
        public void CompareTest()
        {
#pragma warning disable CS1718 // Comparison made to same variable
            // ReSharper disable once EqualExpressionComparison
            var match = BitFlagSample.Ok == BitFlagSample.Ok;
#pragma warning restore CS1718 // Comparison made to same variable
            match.Should().Be(true);

            match = BitFlagSample.Ok == BitFlagSample.Uninitialized;
            match.Should().Be(false);

            match = BitFlagSample.Ok.Equals(BitFlagSample.Ok);
            match.Should().Be(true);

            match = BitFlagSample.Ok.Equals(BitFlagSample.UnexpectedError);
            match.Should().Be(false);

        }

        [Fact]
        public void Testing()
        {
            var poop = test.GetAll();
            poop.Count().Should().Be(2);
        }

        private class test : EnumAsClass<test>
        {
            public static test Test1 = new test("hello");
            public static test Test2 = new test("hello2");

            private test(string name) : base(name)
            {
            }


            //private test(int index, string name) : base(index, name)
            //{
            //}
        }
    }
}
