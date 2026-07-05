using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class EnsureCapacityTests
    : EnsureCapacityTestsBase<ObservableHashSet<int>>
{
    protected override ObservableHashSet<int> CreateUut(int initialCapacity)
        => new(capacity: initialCapacity);

    protected override void EnsureCapacity(
            ObservableHashSet<int>  uut,
            int                     capacity)
        => uut.EnsureCapacity(capacity);

    protected override int GetCapacity(ObservableHashSet<int> uut)
        => uut.Capacity;
}
