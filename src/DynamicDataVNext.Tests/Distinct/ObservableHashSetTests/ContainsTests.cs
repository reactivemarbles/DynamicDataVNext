using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class ContainsTests
    : Distinct.SetTestBases.ContainsTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
