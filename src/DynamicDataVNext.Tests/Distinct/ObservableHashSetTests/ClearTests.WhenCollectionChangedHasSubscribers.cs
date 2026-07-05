using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.ClearTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
