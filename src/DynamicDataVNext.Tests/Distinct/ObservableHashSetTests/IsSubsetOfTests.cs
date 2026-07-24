using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class IsSubsetOfTests
    : Distinct.SetTestBases.IsSubsetOfTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
