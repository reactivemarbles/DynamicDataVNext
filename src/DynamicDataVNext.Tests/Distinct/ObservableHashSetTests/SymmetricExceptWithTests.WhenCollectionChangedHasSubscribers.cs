using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class SymmetricExceptWithTests
{
    [TestFixture]
    public class WhenCollectionChangedHasSubscribers
        : Distinct.SymmetricExceptWithTests.Base<UutFixture.WhenSetChangedHasSubscribers, ObservableHashSet<int>>;
}
