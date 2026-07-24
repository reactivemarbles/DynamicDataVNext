using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class ContainsTests
    : Distinct.SetTestBases.ContainsTests.Base<UutFixture, ReactiveHashSet<int>>;
