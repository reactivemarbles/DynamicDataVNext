namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class ContainsKeyTests
    : Keyed.DictionaryTestBases.ContainsKeyTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
