using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

[TestFixture]
public class CreateForAdditionTests
{
    [Test]
    public void Always_ResultIsUpdate()
    {
        var item = 1;
    
        var result = DistinctChangeSet.CreateForAddition(item);
            
        result.Should().BeValid();
        result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
        result.Changes.Should().ContainSingle("a single added item was given");
        result.Changes[0].Type.Should().Be(DistinctChangeType.Addition, "a single added item was given");
        result.Changes[0].Item.Should().Be(item, "a single added item was given");
    }
}
