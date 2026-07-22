using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class TryGetValueTests
    : Keyed.TryGetValueTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
