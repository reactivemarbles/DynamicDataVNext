using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class IsSubsetOfTests
    : Distinct.IsSubsetOfTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
