namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ResetTests
{
    public partial class ForItems
    {
        [TestFixture]
        public class WhenChangeStreamSourceHasSubscribers
            : Keyed.DictionaryTestBases.ResetTests.ForItemsBase<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>;
    }
}
