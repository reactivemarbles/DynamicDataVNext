using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ExceptWithTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.SetTestBases.ExceptWithTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
