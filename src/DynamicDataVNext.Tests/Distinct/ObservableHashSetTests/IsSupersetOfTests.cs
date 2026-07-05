using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class IsSupersetOfTests
    : Distinct.IsSupersetOfTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
