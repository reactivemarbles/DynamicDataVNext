using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class IsSupersetOfTests
    : Distinct.IsSupersetOfTests.Base<UutFixture, ReactiveHashSet<int>>;
