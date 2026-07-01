using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ContainsTests
    : Distinct.ContainsTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
