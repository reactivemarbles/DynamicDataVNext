using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class SymmetricExceptWithTests
    : Distinct.SymmetricExceptWithTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
