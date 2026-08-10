namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

[TestFixture]
public class IsProperSupersetOfTests
    : Distinct.SetTestBases.IsProperSupersetOfTests.Base<UutFixture, ReactiveHashSet<int>>;
