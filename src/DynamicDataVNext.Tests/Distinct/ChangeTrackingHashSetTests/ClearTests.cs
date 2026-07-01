using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ClearTests
    : Distinct.ClearTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
