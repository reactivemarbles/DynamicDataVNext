using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class ContainsKeyTests
    : Keyed.ContainsKeyTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
