using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.KeyedChangeSetTests;

[TestFixture]
public class EmptyTests
{
    [Test]
    public void Always_IsEmpty()
    {
        var result = KeyedChangeSet.Empty<int, int>();
        
        result.Should().BeValid();
        result.Type.Should().Be(ChangeSetType.Empty, "an uninitialized changeset should be empty");
    }
}        
