using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class SymmetricExceptWithTests
{
    [TestFixture]
    public class WhenNoSubscriptionsAreActive
        : Distinct.SymmetricExceptWithTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
}
