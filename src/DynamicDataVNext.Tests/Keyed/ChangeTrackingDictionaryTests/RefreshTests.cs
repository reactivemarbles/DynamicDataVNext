using NUnit.Framework;

namespace DynamicDataVNext.Tests.Keyed.ChangeTrackingDictionaryTests;

[TestFixture]
public class RefreshTests
    : Keyed.RefreshTests.Base<UutFixture, ChangeTrackingDictionary<string, int>>;
