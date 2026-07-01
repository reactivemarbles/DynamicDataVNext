using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class IsProperSupersetOfTests
    : Distinct.IsProperSupersetOfTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
