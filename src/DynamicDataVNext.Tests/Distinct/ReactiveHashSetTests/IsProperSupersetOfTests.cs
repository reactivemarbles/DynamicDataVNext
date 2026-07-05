using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class IsProperSupersetOfTests
    : Distinct.IsProperSupersetOfTests.Base<UutFixture, ReactiveHashSet<int>>;
