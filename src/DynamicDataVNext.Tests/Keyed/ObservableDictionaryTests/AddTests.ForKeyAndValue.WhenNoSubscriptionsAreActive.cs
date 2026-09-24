namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class AddTests
{
    public partial class ForKeyAndValue
    {
        [TestFixture]
        public class WhenNoSubscriptionsAreActive
            : Keyed.DictionaryTestBases.AddTests.ForKeyAndValueBase<UutFixture.WhenNoSubscriptionsAreActive, ObservableDictionary<string, int>>;
    }
}
