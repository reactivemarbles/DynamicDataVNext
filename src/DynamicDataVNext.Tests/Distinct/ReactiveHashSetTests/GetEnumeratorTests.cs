using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class GetEnumeratorTests
    : Distinct.GetEnumeratorTests.Base<UutFixture, ReactiveHashSet<int>>;
