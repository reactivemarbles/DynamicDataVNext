namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class IndexerTests
{
    public partial class Set
    {
        [TestFixture]
        public class WhenCollectionChangedHasSubscribers
            : Keyed.DictionaryTestBases.IndexerTests.Set.Base<UutFixture.WhenCollectionChangedHasSubscribers, ObservableDictionary<string, int>>;
    }
}
