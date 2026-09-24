namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class RemoveTests
{
    public partial class ForKeyValuePair
    {
        [TestFixture]
        public class WhenChangeStreamSourceHasSubscribers
            : Keyed.DictionaryTestBases.RemoveTests.ForKeyValuePairBase<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>;
    }
}
