using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class IsSubsetOfTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlySetUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlySet<int>
    {
        [Test]
        public void WhenOtherIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(Enumerable.Empty<int>());

            var result = fixture.Uut.Invoking(uut => uut.IsSubsetOf(null!))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("other")
                .Which;
            
            Console.WriteLine(result);
        }
        
        [TestCaseSource(typeof(IsSubsetOfTests), nameof(WhenSetIsNotSubsetOfOther_TestCases))]
        public void WhenSetIsNotSubsetOfOther_ReturnsFalse(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            var result = fixture.Uut.IsSubsetOf(testCase.Other);
            
            result.Should().BeFalse(testCase.Because);
        }

        [TestCaseSource(typeof(IsSubsetOfTests), nameof(WhenSetIsSubsetOfOther_TestCases))]
        public void WhenSetIsSubsetOfOther_ReturnsTrue(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            var result = fixture.Uut.IsSubsetOf(testCase.Other);
            
            result.Should().BeTrue(testCase.Because);
        }
    }
}
