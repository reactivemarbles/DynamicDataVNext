namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

[TestFixture]
public class InsertTests
    : Ordered.ListTestBases.InsertTests.Base<UutFixture, ChangeTrackingList<string?>>;
