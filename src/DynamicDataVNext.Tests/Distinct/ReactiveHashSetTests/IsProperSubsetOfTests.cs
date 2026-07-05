using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class IsProperSubsetOfTests
    : Distinct.IsProperSubsetOfTests.Base<UutFixture, ReactiveHashSet<int>>;
