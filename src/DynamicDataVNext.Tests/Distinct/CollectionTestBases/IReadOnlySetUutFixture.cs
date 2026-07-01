using System;
using System.Collections.Generic;

namespace DynamicDataVNext.Tests.Distinct;

public interface IReadOnlySetUutFixture<out TUutFixture, out TUut>
        : IDisposable
    where TUutFixture : IReadOnlySetUutFixture<TUutFixture, TUut>
    where TUut : IReadOnlySet<int>
{
    static abstract TUutFixture Create(
        IEnumerable<int>        items,
        IEqualityComparer<int>? comparer    = null,
        DistinctItemOptions     options     = default);
    
    TUut Uut { get; }
    
    IEqualityComparer<int> UutComparer { get; }
    
    DistinctItemOptions UutOptions { get; }
}
