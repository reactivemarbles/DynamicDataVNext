namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class RemoveTests
    : Ordered.ListTestBases.RemoveTests.Base<UutFixture, ChangeTrackingList<string?>>;
