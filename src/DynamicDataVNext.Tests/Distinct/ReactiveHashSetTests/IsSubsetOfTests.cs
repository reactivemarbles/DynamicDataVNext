namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class IsSubsetOfTests
    : Distinct.SetTestBases.IsSubsetOfTests.Base<UutFixture, ReactiveHashSet<int>>;
