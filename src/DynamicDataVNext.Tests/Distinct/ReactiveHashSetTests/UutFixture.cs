using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using DynamicDataVNext.Tests.Distinct.SetTestBases;

namespace DynamicDataVNext.Tests.Distinct.ReactiveHashSetTests;

public sealed class UutFixture
    : IReadOnlySetUutFixture<UutFixture, ReactiveHashSet<int>>
{
    public static UutFixture Create(
            IEnumerable<int>        items,
            IEqualityComparer<int>? comparer    = null,
            DistinctItemOptions     options     = default)
        => new(
            items:      items,
            comparer:   comparer,
            options:    options);
    
    private UutFixture(
        IEnumerable<int>        items,
        IEqualityComparer<int>? comparer,
        DistinctItemOptions     options)
    {
        var initialItems = items.ToArray();
        
        _uut = new(
            source:     (initialItems.Length is 0)
                ? Observable.Empty<DistinctChangeSet<int>>()
                : Observable.Return(DistinctChangeSet.CreateForReset(addedItems: initialItems)),
            comparer:   comparer,
            options:    options);
    }

    public ReactiveHashSet<int> Uut
        => _uut;

    public IEqualityComparer<int> UutComparer
        => _uut.ChangeStream.Comparer;
    
    public DistinctItemOptions UutOptions
        => _uut.ChangeStream.Options;
    
    public void Dispose()
        => _uut.Dispose();
    
    private readonly ReactiveHashSet<int> _uut;
}
