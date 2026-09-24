namespace DynamicDataVNext.Tests.Keyed.ReactiveDictionaryTests;

[TestFixture]
public class TryGetValueTests
    : Keyed.DictionaryTestBases.TryGetValueTests.Base<UutFixture, ReactiveDictionary<string, int>>;
