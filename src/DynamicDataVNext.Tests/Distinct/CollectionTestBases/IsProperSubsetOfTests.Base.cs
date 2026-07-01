using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class IsProperSubsetOfTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlySetUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlySet<int>
    {
        [Test]
        public void WhenOtherIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(Enumerable.Empty<int>());

            var result = fixture.Uut.Invoking(uut => uut.IsProperSubsetOf(null!))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("other")
                .Which;
            
            Console.WriteLine(result);
        }

        [TestCaseSource(typeof(IsProperSubsetOfTests), nameof(WhenSetIsNotProperSubsetOfOther_TestCases))]
        public void WhenSetIsNotProperSubsetOfOther_ReturnsFalse(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            var result = fixture.Uut.IsProperSubsetOf(testCase.Other);
                
            result.Should().BeFalse(testCase.Because);
        }
        
        [TestCaseSource(typeof(IsProperSubsetOfTests), nameof(WhenSetIsProperSubsetOfOther_TestCases))]
        public void WhenSetIsProperSubsetOfOther_ReturnsTrue(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            var result = fixture.Uut.IsProperSubsetOf(testCase.Other);
            
            result.Should().BeTrue(testCase.Because);
        }
    }
}
