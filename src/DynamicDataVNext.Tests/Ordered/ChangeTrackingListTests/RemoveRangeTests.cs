namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class RemoveRangeTests
    : Ordered.ListTestBases.RemoveRangeTests.Base<UutFixture, ChangeTrackingList<string?>>;
