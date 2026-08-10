namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class ContainsTests
    : Keyed.DictionaryTestBases.ContainsTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
