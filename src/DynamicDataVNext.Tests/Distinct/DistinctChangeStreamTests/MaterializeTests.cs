using System;
using System.Reactive.Subjects;

using AwesomeAssertions;
using NUnit.Framework;

namespace DynamicDataVNext.Tests.Distinct.DistinctChangeStreamTests;

[TestFixture]
public class MaterializeTests
{
    [Test]
    public void Always_ResultIsConstructedFromStream()
    {
        using var source = new Subject<DistinctChangeSet<string>>(); 

        var stream = new DistinctChangeStream<string>()
        {
            Comparer = StringComparer.OrdinalIgnoreCase,
            Options  = new DistinctItemOptions()
            {
                ItemsAreMutable = true
            },
            Source   = source
        };
        
        var result = stream.Materialize();
        
        result.Should().NotBeNull();
        result.ChangeStream.Comparer.Should().BeSameAs(stream.Comparer);
        result.ChangeStream.Options.Should().Be(stream.Options);
        
        result.Should().BeEmpty("no items have been added to the collection");
        
        var items = new[] { "1", "2", "3" };
        source.OnNext(DistinctChangeSet.CreateForReset(items));
        
        result.Should().BeEquivalentTo(
            expectation:    items,
            config:         options => options.WithoutStrictOrdering(),
            because:        "items were added to the collection");
    }
}
