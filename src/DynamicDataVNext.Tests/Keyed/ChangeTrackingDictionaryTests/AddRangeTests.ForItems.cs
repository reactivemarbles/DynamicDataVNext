using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class AddRangeTests
{
    [TestFixture]
    public sealed class ForItems
        : Keyed.AddRangeTests.ForItemsBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
