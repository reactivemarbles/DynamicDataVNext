using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class OverlapsTests
    : Distinct.OverlapsTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
