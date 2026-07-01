using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class ResetTests
    : Distinct.ResetTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
