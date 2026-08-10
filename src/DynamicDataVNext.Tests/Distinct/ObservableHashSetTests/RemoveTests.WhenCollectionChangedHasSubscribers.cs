namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RemoveTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.SetTestBases.RemoveTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
