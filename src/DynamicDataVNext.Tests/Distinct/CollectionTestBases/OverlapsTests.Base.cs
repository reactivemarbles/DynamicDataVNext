using System;
using System.Collections.Generic;
using System.Linq;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct;

public static partial class OverlapsTests
{
    public abstract class Base<TUutFixture, TUut>
        where TUutFixture : IReadOnlySetUutFixture<TUutFixture, TUut>
        where TUut : IReadOnlySet<int>
    {
        [Test]
        public void WhenOtherIsNull_ThrowsException()
        {
            using var fixture = TUutFixture.Create(Enumerable.Empty<int>());
            
            var result = fixture.Uut.Invoking(uut => uut.Overlaps(null!))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("other")
                .Which;
            
            Console.WriteLine(result);
        }
        
        [TestCaseSource(typeof(OverlapsTests), nameof(WhenSetDoesNotOverlapOther_TestCases))]
        public void WhenSetDoesNotOverlapOther_ReturnsFalse(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
            
            var result = fixture.Uut.Overlaps(testCase.Other);
            
            result.Should().BeFalse(testCase.Because);
        }
        
        [TestCaseSource(typeof(OverlapsTests), nameof(WhenSetOverlapsOther_TestCases))]
        public void WhenSetOverlapsOther_ReturnsTrue(SetOperationTestCase testCase)
        {
            using var fixture = TUutFixture.Create(items: testCase.Items);
            
            var result = fixture.Uut.Overlaps(testCase.Other);
            
            result.Should().BeTrue(testCase.Because);
        }
    }
}
