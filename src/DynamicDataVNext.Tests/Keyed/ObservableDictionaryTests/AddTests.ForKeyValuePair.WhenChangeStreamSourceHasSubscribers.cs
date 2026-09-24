namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddTests
{
    public partial class ForKeyValuePair
    {
        [TestFixture]
        public class WhenChangeStreamSourceHasSubscribers
            : Keyed.DictionaryTestBases.AddTests.ForKeyValuePairBase<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>;
    }
}
