namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class IsProperSubsetOfTests
    : Distinct.SetTestBases.IsProperSubsetOfTests.Base<UutFixture, ReactiveHashSet<int>>;
