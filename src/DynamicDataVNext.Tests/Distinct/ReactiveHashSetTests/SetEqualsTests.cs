using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class SetEqualsTests
    : Distinct.SetEqualsTests.Base<UutFixture, ReactiveHashSet<int>>;
