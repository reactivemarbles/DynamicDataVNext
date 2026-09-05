namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.SetTestBases.RefreshTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
