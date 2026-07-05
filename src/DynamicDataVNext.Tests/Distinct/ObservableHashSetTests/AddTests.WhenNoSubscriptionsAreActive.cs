using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.AddTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
