using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class CopyToTests
    : Distinct.CopyToTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
