using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.ObservableHashSetTests;

[TestFixture]
public class GetEnumeratorTests
    : Distinct.SetTestBases.GetEnumeratorTests.Base<UutFixture.WhenNoSubscriptionsAreActive, ObservableHashSet<int>>;
