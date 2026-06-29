using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

public static partial class CreateForUpdateTests
{
    [TestFixture]
    public class WithSingleChange
    {
        [Test]
        public void Always_ResultIsUpdate()
        {
            var change = new DistinctChange<int>()
            {
                Item = 1,
                Type = DistinctChangeType.Addition
            };
            
            var result = DistinctChangeSet.CreateForUpdate(change: change);
            
            result.Should().BeValid();
            result.Type.Should().Be(ChangeSetType.Update, "an update operation should have been constructed");
            result.Changes.Should().BeEquivalentTo(new[] { change }, "the given change should be listed");
        }
    }
}
