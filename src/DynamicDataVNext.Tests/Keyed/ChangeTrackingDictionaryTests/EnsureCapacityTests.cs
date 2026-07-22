using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class EnsureCapacityTests
    : EnsureCapacityTestsBase<ChangeTrackingDictionary<string, int>>
{
    protected override ChangeTrackingDictionary<string, int> CreateUut(int initialCapacity)
        => new(capacity: initialCapacity);

    protected override void EnsureCapacity(
            ChangeTrackingDictionary<string, int>   uut,
            int                                     capacity)
        => uut.EnsureCapacity(capacity);

    protected override int GetCapacity(ChangeTrackingDictionary<string, int> uut)
        => uut.Capacity;
}
