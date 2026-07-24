using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class OverlapsTests
    : Distinct.SetTestBases.OverlapsTests.Base<UutFixture, ReactiveHashSet<int>>;
