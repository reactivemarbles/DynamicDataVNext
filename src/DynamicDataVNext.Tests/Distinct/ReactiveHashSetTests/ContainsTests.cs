using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class ContainsTests
    : Distinct.ContainsTests.Base<UutFixture, ReactiveHashSet<int>>;
