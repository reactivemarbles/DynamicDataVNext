namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class InsertRangeTests
    : Ordered.ListTestBases.InsertRangeTests.Base<UutFixture, ChangeTrackingList<string?>>;
