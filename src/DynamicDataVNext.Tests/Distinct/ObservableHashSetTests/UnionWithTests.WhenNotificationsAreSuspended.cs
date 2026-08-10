namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class UnionWithTests
{
    [TestFixture]
    public class WhenNotificationsAreSuspended
        : Distinct.SetTestBases.UnionWithTests.Base<UutFixture.WhenNotificationsAreSuspended, ObservableHashSet<int>>;
}
