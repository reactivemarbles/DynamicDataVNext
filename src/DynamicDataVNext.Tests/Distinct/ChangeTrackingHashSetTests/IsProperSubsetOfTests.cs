using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class IsProperSubsetOfTests
    : Distinct.SetTestBases.IsProperSubsetOfTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
