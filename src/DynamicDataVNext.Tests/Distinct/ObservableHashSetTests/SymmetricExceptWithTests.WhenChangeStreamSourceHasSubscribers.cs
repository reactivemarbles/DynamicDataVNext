using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class SymmetricExceptWithTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.SymmetricExceptWithTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
