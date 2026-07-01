using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class IsProperSubsetOfTests
    : Distinct.IsProperSubsetOfTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
