namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Keyed.DictionaryTestBases.ClearTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
}
