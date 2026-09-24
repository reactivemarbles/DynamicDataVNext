namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Keyed.DictionaryTestBases.RefreshTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableDictionary<string, int>>;
}
