using System.Collections.Generic;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class ContainsTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlySetUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlySet<int>
    {
        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsInSet_TestCases))]
        public void WhenItemIsInSet_ReturnsTrue(ItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.Items);

            var result = fixture.Uut.Contains(testCase.Item);
                
            result.Should().BeTrue("the item is in the initial set of items");
        }

        [TestCaseSource(typeof(ContainsTests), nameof(WhenItemIsNotInSet_TestCases))]
        public void WhenItemIsNotInSet_ReturnsFalse(ItemOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(testCase.Items);

            var result = fixture.Uut.Contains(testCase.Item);
                
            result.Should().BeFalse("the item is not in the initial set of items");
        }
    }
}
