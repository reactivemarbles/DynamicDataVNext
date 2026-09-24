namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class IndexerTests
{
    [TestFixture]
    public sealed class Set
        : Keyed.DictionaryTestBases.IndexerTests.Set.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
}
