namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class AddRangeTests
    : Ordered.ListTestBases.AddRangeTests.Base<UutFixture, ChangeTrackingList<string?>>;
