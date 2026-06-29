using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

public static partial class CreateForUpdateTests
{
    [TestFixture]
    public class WithSingleChange
    {
        [Test]
        public void Always_ResultIsUpdate()
        {
            var change = KeyedChange.CreateAddition(
                key:    1,
                item:   2);
            
            var result = KeyedChangeSet.CreateForUpdate(change: change);
            
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().BeEquivalentTo(new[] { change }, "the given change should be listed");
        }
    }
}
