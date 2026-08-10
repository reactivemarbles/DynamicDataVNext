namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.SetTestBases.ClearTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
