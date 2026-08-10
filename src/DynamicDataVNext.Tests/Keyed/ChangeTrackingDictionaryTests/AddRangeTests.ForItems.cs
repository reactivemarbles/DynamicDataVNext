namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class AddRangeTests
{
    [TestFixture]
    public sealed class ForItems
        : Keyed.DictionaryTestBases.AddRangeTests.ForItemsBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
