using AwesomeAssertions;

using NUnit.Framework;

namespace DynamicDataVNext.Tests.Ordered.OrderedChangeSetTests;

[TestFixture]
public class DefaultTests
{
    [Test]
    public void Always_IsEmpty()
    {
        var result = default(OrderedChangeSet<int>);
        
        result.Should().BeValid();
        result.Type.Should().Be(ChangeSetType.Empty, "an uninitialized changeset should be empty");
    }
}        
