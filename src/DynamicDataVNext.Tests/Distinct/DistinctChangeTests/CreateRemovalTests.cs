using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeTests;

[TestFixture]
public class CreateRemovalTests
{
    [Test]
    public void Always_ResultIsRemoval()
    {
        var item = 1;
        
        var result = DistinctChange.CreateRemoval(item);
        
        result.Category.Should().Be(ChangeCategory.Removal, "a removal change should have been generated");
        result.Type.Should().Be(DistinctChangeType.Removal, "a removal change should have been generated");
        result.Item.Should().Be(item, "the given item should have been embedded into the generated change");
    }
}
