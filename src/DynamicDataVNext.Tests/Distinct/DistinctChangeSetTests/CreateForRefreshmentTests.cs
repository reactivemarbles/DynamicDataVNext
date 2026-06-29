using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

[TestFixture]
public class CreateForRefreshmentTests
{
    [Test]
    public void Always_ResultIsUpdate()
    {
        var item = 1;
    
        var result = DistinctChangeSet.CreateForRefreshment(item);
            
        result.Should().BeValid();
        result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
        result.Changes.Should().ContainSingle("a single refreshed item was given");
        result.Changes[0].Type.Should().Be(DistinctChangeType.Refreshment, "a single refreshed item was given");
        result.Changes[0].Item.Should().Be(item, "a single refreshed item was given");
    }
}
