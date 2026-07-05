using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class ClearTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.ClearTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
