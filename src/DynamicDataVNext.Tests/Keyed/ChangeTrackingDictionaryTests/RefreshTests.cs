using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class RefreshTests
    : Keyed.DictionaryTestBases.RefreshTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
