using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ExceptWithTests
    : Distinct.SetTestBases.ExceptWithTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
