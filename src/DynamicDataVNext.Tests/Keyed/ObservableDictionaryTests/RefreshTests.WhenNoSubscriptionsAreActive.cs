namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Keyed.DictionaryTestBases.RefreshTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
}
