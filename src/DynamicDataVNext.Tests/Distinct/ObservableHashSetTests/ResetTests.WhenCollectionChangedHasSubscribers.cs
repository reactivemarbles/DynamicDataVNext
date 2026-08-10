namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ResetTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.SetTestBases.ResetTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
