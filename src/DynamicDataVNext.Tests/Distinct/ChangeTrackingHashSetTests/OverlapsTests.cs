using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class OverlapsTests
    : Distinct.SetTestBases.OverlapsTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
