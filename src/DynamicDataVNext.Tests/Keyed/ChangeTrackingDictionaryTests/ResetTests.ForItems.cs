namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class ResetTests
{
    public class ForItems
        : DictionaryTestBases.ResetTests.ForItemsBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
