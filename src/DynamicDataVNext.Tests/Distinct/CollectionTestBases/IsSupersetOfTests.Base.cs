using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class IsSupersetOfTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlySetUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlySet<int>
    {
        [Test]
        public void WhenOtherIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(Enumerable.Empty<int>());

            var result = fixture.Uut.Invoking(uut => uut.IsSupersetOf(null!))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("other")
                .Which;
            
            Console.WriteLine(result);
        }
        
        [TestCaseSource(typeof(IsSupersetOfTests), nameof(WhenSetIsNotSupersetOfOther_TestCases))]
        public void WhenSetIsNotSupersetOfOther_ReturnsFalse(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            var result = fixture.Uut.IsSupersetOf(testCase.Other);
            
            result.Should().BeFalse(testCase.Because);
        }
        
        [TestCaseSource(typeof(IsSupersetOfTests), nameof(WhenSetIsSupersetOfOther_TestCases))]
        public void WhenSetIsSupersetOfOther_ReturnsTrue(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);

            var result = fixture.Uut.IsSupersetOf(testCase.Other);
            
            result.Should().BeTrue(testCase.Because);
        }
    }
}
