using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.SetTestBases.AddTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
