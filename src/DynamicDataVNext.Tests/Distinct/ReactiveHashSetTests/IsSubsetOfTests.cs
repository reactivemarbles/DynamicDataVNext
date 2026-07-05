using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class IsSubsetOfTests
    : Distinct.IsSubsetOfTests.Base<UutFixture, ReactiveHashSet<int>>;
