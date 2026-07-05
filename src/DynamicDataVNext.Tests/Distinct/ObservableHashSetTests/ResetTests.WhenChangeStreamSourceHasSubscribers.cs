using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ResetTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.ResetTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
