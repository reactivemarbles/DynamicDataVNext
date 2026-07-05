using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class RemoveTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.RemoveTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
