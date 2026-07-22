using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class ContainsTests
    : Keyed.ContainsTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
