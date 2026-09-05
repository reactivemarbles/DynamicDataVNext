namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

public static partial class ResetTests
{
    public class ForValuesAndKeySelector
        : DictionaryTestBases.ResetTests.ForValuesAndKeySelectorBase<UutFixture, ChangeTrackingDictionary<string, int>>;
}
