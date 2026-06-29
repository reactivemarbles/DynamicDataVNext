using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

[TestFixture]
public class CreateForRemovalTests
{
    [Test]
    public void Always_ResultIsUpdate()
    {
        var item = 1;
    
        var result = DistinctChangeSet.CreateForRemoval(item);
            
        result.Should().BeValid();
        result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
        result.Changes.Should().ContainSingle("a single removed item was given");
        result.Changes[0].Type.Should().Be(DistinctChangeType.Removal, "a single removed item was given");
        result.Changes[0].Item.Should().Be(item, "a single removed item was given");
    }
}
