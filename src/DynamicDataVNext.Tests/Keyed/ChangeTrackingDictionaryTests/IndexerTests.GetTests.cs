namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class IndexerTests
{
    [TestFixture]
    public sealed class GetTests
        : Keyed.DictionaryTestBases.IndexerTests.GetTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
}
