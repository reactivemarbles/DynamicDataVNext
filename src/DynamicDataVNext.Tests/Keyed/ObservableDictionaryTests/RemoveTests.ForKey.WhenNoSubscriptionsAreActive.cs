namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class RemoveTests
{
    public partial class ForKey
    {
        [TestFixture]
        public class WhenNoSubscriptionsAreActive
            : Keyed.DictionaryTestBases.RemoveTests.ForKeyBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
    }
}
