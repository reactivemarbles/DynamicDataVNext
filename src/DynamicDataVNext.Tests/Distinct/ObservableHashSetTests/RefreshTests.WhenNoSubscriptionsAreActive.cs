namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.SetTestBases.RefreshTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
