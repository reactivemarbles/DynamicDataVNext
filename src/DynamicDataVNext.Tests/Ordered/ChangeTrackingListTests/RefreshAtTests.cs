namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class RefreshAtTests
    : Ordered.ListTestBases.RefreshAtTests.Base<UutFixture, ChangeTrackingList<string?>>;
