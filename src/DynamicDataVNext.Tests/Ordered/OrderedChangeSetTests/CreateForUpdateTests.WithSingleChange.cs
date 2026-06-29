using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

public static partial class CreateForUpdateTests
{
    [TestFixture]
    public class WithSingleChange
    {
        [Test]
        public void Always_ResultIsUpdate()
        {
            var change = OrderedChange.CreateInsertion(
                index:  0,
                item:   1);
            
            var result = OrderedChangeSet.CreateForUpdate(change: change);
            
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().BeEquivalentTo(new[] { change }, "the given change should be listed");
        }
    }
}
