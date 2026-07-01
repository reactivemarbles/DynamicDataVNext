using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class RemoveTests
    : Distinct.RemoveTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
