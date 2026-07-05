using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class UnionWithTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.UnionWithTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
