using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class SetEqualsTests
    : Distinct.SetEqualsTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
