namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class TryGetValueTests
    : Keyed.DictionaryTestBases.TryGetValueTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
