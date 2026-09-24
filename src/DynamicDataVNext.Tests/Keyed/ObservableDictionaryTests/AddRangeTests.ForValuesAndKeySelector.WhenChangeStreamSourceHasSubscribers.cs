namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddRangeTests
{
    public partial class ForValuesAndKeySelector
    {
        [TestFixture]
        public class WhenChangeStreamSourceHasSubscribers
            : Keyed.DictionaryTestBases.AddRangeTests.ForValuesAndKeySelectorBase<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>;
    }
}
