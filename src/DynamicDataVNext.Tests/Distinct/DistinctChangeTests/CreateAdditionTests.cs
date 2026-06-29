using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeTests;

[TestFixture]
public class CreateAdditionTests
{
    [Test]
    public void Always_ResultIsAddition()
    {
        var item = 1;
        
        var result = DistinctChange.CreateAddition(item);
        
        result.Category.Should().Be(ChangeCategory.Addition, "an addition change should have been generated");
        result.Type.Should().Be(DistinctChangeType.Addition, "an addition change should have been generated");
        result.Item.Should().Be(item, "the given item should have been embedded into the generated change");
    }
}
