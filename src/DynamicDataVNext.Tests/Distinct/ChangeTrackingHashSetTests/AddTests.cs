using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class AddTests
    : Distinct.SetTestBases.AddTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
