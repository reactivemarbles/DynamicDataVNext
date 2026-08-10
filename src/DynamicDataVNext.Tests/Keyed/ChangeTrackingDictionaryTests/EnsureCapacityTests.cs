namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class EnsureCapacityTests
    : EnsureCapacityTestsBase<ChangeTrackingDictionary<string, int>>
{
    protected override ChangeTrackingDictionary<string, int> CreateUut(int initialCapacity)
        => new(capacity: initialCapacity);
}
