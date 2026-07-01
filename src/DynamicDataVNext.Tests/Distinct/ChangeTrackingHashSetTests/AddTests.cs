using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class AddTests
    : Distinct.AddTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
