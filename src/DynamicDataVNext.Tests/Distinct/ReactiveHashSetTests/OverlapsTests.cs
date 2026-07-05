using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class OverlapsTests
    : Distinct.OverlapsTests.Base<UutFixture, ReactiveHashSet<int>>;
