namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class RemoveAtTests
    : Ordered.ListTestBases.RemoveAtTests.Base<UutFixture, ChangeTrackingList<string?>>;
