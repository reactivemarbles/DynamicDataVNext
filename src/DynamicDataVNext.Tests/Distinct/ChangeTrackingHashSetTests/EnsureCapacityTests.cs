using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class EnsureCapacityTests
    : EnsureCapacityTestsBase<ChangeTrackingHashSet<int>>
{
    protected override ChangeTrackingHashSet<int> CreateUut(int initialCapacity)
        => new(capacity: initialCapacity);
}
