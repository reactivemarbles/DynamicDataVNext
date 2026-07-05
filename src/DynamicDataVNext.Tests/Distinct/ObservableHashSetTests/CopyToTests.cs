using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class CopyToTests
    : Distinct.CopyToTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
