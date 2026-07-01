using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ExceptWithTests
    : Distinct.ExceptWithTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
