using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class IsSubsetOfTests
    : Distinct.IsSubsetOfTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
