using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class ContainsTests
    : Distinct.ContainsTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
