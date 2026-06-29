using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeSetTests;

[TestFixture]
public class DefaultTests
{
    [Test]
    public void Always_IsEmpty()
    {
        var result = default(DistinctChangeSet<int>);
        
        result.Should().BeValid();
        result.Type.Should().Be(ChangeSetType.Empty, "an uninitialized changeset should be empty");
    }
}        
