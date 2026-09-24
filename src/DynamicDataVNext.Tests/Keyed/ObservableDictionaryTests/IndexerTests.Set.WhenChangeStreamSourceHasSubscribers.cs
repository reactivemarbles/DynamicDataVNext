namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class IndexerTests
{
    public partial class Set
    {
        [TestFixture]
        public class WhenChangeStreamSourceHasSubscribers
            : Keyed.DictionaryTestBases.IndexerTests.Set.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>;
    }
}
