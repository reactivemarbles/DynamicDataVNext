using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class IsSubsetOfTests
    : Distinct.SetTestBases.IsSubsetOfTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
