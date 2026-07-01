using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ChangeTrackingHashSetTests;

[TestFixture]
public class GetEnumeratorTests
    : Distinct.GetEnumeratorTests.Base<UutFixture, ChangeTrackingHashSet<int>>;
