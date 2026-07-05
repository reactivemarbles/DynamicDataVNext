using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ExceptWithTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.ExceptWithTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
