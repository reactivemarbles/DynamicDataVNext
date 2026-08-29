namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class EnsureCapacityTests
    : EnsureCapacityTestsBase<ChangeTrackingList<string?>>
{
    protected override ChangeTrackingList<string?> CreateUut(int initialCapacity)
        => new(capacity: initialCapacity);
}
