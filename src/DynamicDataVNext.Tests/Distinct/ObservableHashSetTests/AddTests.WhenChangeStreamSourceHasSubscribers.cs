using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

public partial class AddTests
{
    [TestFixture]
    public class WhenChangeStreamSourceHasSubscribers
        : Distinct.SetTestBases.AddTests.Base<UutFixture.WhenChangeStreamSourceHasSubscribers, ObservableHashSet<int>>;
}
