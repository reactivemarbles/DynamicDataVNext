using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class IsSupersetOfTests
    : Distinct.SetTestBases.IsSubsetOfTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
