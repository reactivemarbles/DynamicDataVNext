using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class SetEqualsTests
    : Distinct.SetEqualsTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
