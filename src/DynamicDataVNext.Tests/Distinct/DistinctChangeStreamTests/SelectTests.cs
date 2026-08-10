using System;
using System.Collections.Generic;

using ReactiveUI.Primitives.Signals;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

[TestFixture]
public partial class SelectTests
{
    [Test]
    public void WhenComparerIsGiven_ComparerPropagates()
    {
        var stream = new DistinctChangeStream<string>()
        {
            Comparer    = EqualityComparer<string>.Default,
            Source      = Signal.Never<DistinctChangeSet<string>>()
        };
        
        var result = stream.Select(
            selector:   static item => item,
            comparer:   StringComparer.OrdinalIgnoreCase);
        
        result.Comparer.Should().BeSameAs(StringComparer.OrdinalIgnoreCase, "a given comparer should propagate downstream");
    }
    
    [Test]
    public void WhenComparerIsNotGiven_DefaultComparerPropagates()
    {
        var stream = new DistinctChangeStream<string>()
        {
            Comparer    = StringComparer.OrdinalIgnoreCase,
            Source      = Signal.Never<DistinctChangeSet<string>>()
        };
        
        var result = stream.Select(static item => item);
        
        result.Comparer.Should().BeSameAs(EqualityComparer<string>.Default, "when no comparer is given, the default comparer should be used");
    }
}
