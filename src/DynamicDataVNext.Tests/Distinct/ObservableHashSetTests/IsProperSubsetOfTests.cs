using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class IsProperSubsetOfTests
    : Distinct.IsProperSubsetOfTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
