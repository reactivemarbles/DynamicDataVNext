using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class ConstructorTests
    : Distinct.ConstructorTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
