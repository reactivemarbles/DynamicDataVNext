using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ResetTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.ResetTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
