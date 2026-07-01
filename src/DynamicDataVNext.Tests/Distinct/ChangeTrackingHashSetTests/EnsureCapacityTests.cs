using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class EnsureCapacityTests
    : EnsureCapacityTestsBase<ChangeTrackingHashSet<int>>
{
    protected override ChangeTrackingHashSet<int> CreateUut(int initialCapacity)
        => new(capacity: initialCapacity);

    protected override void EnsureCapacity(
            ChangeTrackingHashSet<int>  uut,
            int                         capacity)
        => uut.EnsureCapacity(capacity);

    protected override int GetCapacity(ChangeTrackingHashSet<int> uut)
        => uut.Capacity;
}
