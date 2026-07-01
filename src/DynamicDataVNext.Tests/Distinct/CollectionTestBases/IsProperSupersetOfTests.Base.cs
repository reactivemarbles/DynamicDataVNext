using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class IsProperSupersetOfTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlySetUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlySet<int>
    {
        [Test]
        public void WhenOtherIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(Enumerable.Empty<int>());

            var result = fixture.Uut.Invoking(uut => uut.IsProperSupersetOf(null!))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("other")
                .Which;
            
            Console.WriteLine(result);
        }
        
        [TestCaseSource(typeof(IsProperSupersetOfTests), nameof(WhenSetIsNotProperSupersetOfOther_TestCases))]
        public void WhenSetIsNotProperSupersetOfOther_ReturnsFalse(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            var result = fixture.Uut.IsProperSupersetOf(testCase.Other);
            
            result.Should().BeFalse(testCase.Because);
        }
        
        [TestCaseSource(typeof(IsProperSupersetOfTests), nameof(WhenSetIsProperSupersetOfOther_TestCases))]
        public void WhenSetIsProperSupersetOfOther_ReturnsTrue(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            var result = fixture.Uut.IsProperSupersetOf(testCase.Other);
            
            result.Should().BeTrue(testCase.Because);
        }
    }
}
