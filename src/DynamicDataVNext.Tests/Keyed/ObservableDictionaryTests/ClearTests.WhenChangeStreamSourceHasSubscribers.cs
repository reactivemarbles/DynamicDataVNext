namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Keyed.DictionaryTestBases.ClearTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>;
}
