using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class IsProperSupersetOfTests
    : Distinct.IsProperSupersetOfTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
