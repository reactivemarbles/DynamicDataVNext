namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class IndexerTests
{
    [TestFixture]
    public sealed class SetTests
        : Keyed.DictionaryTestBases.IndexerTests.SetTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
}
