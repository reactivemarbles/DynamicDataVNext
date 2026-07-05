using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class IntersectWithTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.IntersectWithTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
