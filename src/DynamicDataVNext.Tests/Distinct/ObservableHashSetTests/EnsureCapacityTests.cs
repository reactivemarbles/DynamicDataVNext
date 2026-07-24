using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class EnsureCapacityTests
    : EnsureCapacityTestsBase<ObservableHashSet<int>>
{
    protected override ObservableHashSet<int> CreateUut(int initialCapacity)
        => new(capacity: initialCapacity);
}
