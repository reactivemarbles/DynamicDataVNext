namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ExceptWithTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.SetTestBases.ExceptWithTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
