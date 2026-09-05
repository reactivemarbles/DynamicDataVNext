namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RefreshTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.SetTestBases.RefreshTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
