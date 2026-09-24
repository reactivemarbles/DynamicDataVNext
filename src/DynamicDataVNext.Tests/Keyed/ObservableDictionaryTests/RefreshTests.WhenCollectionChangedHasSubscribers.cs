namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Keyed.DictionaryTestBases.RefreshTests.Base<UutFixture.WhenCollectionChangedHasSubscribers, ObservableDictionary<string, int>>;
}
