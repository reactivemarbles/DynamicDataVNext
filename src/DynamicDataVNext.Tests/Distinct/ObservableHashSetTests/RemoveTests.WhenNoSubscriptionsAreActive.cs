namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RemoveTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.SetTestBases.RemoveTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
