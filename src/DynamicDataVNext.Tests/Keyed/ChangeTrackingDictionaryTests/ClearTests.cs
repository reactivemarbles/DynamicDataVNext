namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class ClearTests
    : Keyed.DictionaryTestBases.ClearTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
