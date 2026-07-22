using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class CopyToTests
    : Keyed.CopyToTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
