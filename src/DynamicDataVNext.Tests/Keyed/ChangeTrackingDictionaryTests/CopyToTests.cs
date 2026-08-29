namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class CopyToTests
    : Keyed.DictionaryTestBases.CopyToTestsBase<UutFixture, ChangeTrackingDictionary<string, int>>;
