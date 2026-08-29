namespace DynamicDataVNext.Tests.Ordered.ChangeTrackingListTests;

public static partial class IndexerTests
{
    [TestFixture]
    public sealed class SetTests
        : Ordered.ListTestBases.IndexerTests.SetTestsBase<UutFixture, ChangeTrackingList<string?>>;
}
