using FluentAssertions;
using Utilities.Examples;
using Utilities.Examples.Enummeration;
using Xunit;

namespace Utilities.Tests.Enumeration
{
    public class FunctionalTests
    {
        [Fact]
        public void BitFlagTest()
        {
            var result = Enummeration.AsBitFlags<BitFlagSample>(9);
            result.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.Contain(BitFlagSample.Uninitialized)
                .And.Contain(BitFlagSample.InvalidState);
        }

        [Fact]
        public void NoMatchShouldExecuteMethodAndReturnNoMatchResult()
        {
            ((SimpleSample) 99).Should().Be(SimpleSample.SimpleSampleIndex1);

            var testAction = (SimpleSample)99;
            testAction.NoMatchExecuted.Should().BeTrue();
        }

        [Fact]
        public void GetAllTest()
        {
            Enummeration.GetAll<BitFlagSample>()
                .Should().NotBeEmpty()
                .And.HaveCount(6)
                .And.OnlyHaveUniqueItems();
        }
    }
}
