using FluentAssertions;
using Utilities.Examples;
using Xunit;

namespace Utilities.Tests.Enumeration
{
    public class DuplicateTests
    {
        internal class Duplicate : EnumAsClass
        {
            public static readonly Duplicate DuplicateIndex1 = new Duplicate(1, "Same Index A");
            public static readonly Duplicate DuplicateIndex2 = new Duplicate(1, "Same Index B");

            public static readonly Duplicate DuplicateName1 = new Duplicate(2, "Same Name");
            public static readonly Duplicate DuplicateName2 = new Duplicate(3, "Same Name");

            public static readonly Duplicate DuplicateEverything1 = new Duplicate(5, "Same Everything");
            public static readonly Duplicate DuplicateEverything2 = new Duplicate(5, "Same Everything");

            private Duplicate(int index, string name) : base(index, name)
            {
                NoMatchResult = () => null;
                MultipleResultSameAsNoMatch = false;
                ExecuteWhenNoMatch = null;
            }

            public static implicit operator Duplicate(int index) => FromIndex<Duplicate>(index);
            public static implicit operator Duplicate(string name) => FromName<Duplicate>(name);
        }


        [Fact]
        public void DuplicateTest()
        {
            ((Duplicate)1).Should().Be(Duplicate.DuplicateIndex1);
            ((Duplicate)"Same Name").Should().Be(Duplicate.DuplicateName1);
            ((Duplicate)5).Should().Be(Duplicate.DuplicateEverything1);

        }
    }
}
