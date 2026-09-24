namespace DynamicDataVNext.Tests.Keyed.ObservableDictionaryTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Keyed.DictionaryTestBases.ClearTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableDictionary<string, int>>;
}
