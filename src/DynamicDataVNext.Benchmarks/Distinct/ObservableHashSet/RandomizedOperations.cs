using System.Collections.Immutable;

using ReactiveUI.Primitives;

using BenchmarkDotNet.Attributes;

namespace DynamicDataVNext.Benchmarks.Distinct.ObservableHashSet;

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class Distinct_ObservableHashSet_RandomizedOperations
{
    public Distinct_ObservableHashSet_RandomizedOperations()
        => _mutations = IntegerMutations.Generate();
    
    [Benchmark(Baseline = true)]
    public void WithoutSubscriptions()
    {
        using var collection = new ObservableHashSet<int>();

        foreach (var mutation in _mutations)
            mutation.ApplyTo(collection);
    }

    [Benchmark]
    public void WithBothSubscriptions()
    {
        using var collection = new ObservableHashSet<int>();

        using var collectionChangedSubscription = collection.CollectionChanged.Subscribe();
        
        using var collectionSubscription = collection.ChangeStream.Source.Subscribe();
        
        foreach (var mutation in _mutations)
            mutation.ApplyTo(collection);
    }

    [Benchmark]
    public void WithCollectionChangedSubscription()
    {
        using var collection = new ObservableHashSet<int>();

        using var collectionChangedSubscription = collection.CollectionChanged.Subscribe();
        
        foreach (var mutation in _mutations)
            mutation.ApplyTo(collection);
    }
    
    [Benchmark]
    public void WithCollectionSubscription()
    {
        using var collection = new ObservableHashSet<int>();

        using var collectionSubscription = collection.ChangeStream.Source.Subscribe();
        
        foreach (var mutation in _mutations)
            mutation.ApplyTo(collection);
    }

    private readonly ImmutableArray<MutationBase<int>> _mutations;
}
