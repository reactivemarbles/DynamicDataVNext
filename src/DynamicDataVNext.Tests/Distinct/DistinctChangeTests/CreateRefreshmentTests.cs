using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeTests;

[TestFixture]
public class CreateRefreshmentTests
{
    [Test]
    public void Always_ResultIsRefreshment()
    {
        var item = 1;
        
        var result = DistinctChange.CreateRefreshment(item);
        
        result.Category.Should().Be(ChangeCategory.Other, "a refreshment change should have been generated");
        result.Type.Should().Be(DistinctChangeType.Refreshment, "a refreshment change should have been generated");
        result.Item.Should().Be(item, "the given item should have been embedded into the generated change");
    }
}
