namespace DynamicDataVNext.Benchmarks.Ordered.ChangeTrackingList;

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class AddRange
{
    static AddRange()
    {
        _initialItemSetsByCount = new();
        _mutationsByItemCount   = new();

        var randomizer = new Randomizer(1234567);

        foreach (var count in new[] { 0, 1, 10, 100, 1_000, 10_000 })
        {
            _initialItemSetsByCount.Add(
                key:    count,
                value:  Enumerable.Repeat(randomizer, count)
                    .Select(static randomizer => randomizer.Int())
                    .ToImmutableArray());

            _mutationsByItemCount.Add(
                key:    count,
                value:  new AddRangeMutation<ChangeTrackingList<int>, int>()
                {
                    Items = Enumerable.Repeat(randomizer, count)
                        .Select(static randomizer => randomizer.Int())
                        .ToImmutableArray()
                });
        }
    }

    [Params(0, 1, 10, 100, 1_000, 10_000)]
    public int InitialItemCount { get; set; }
    
    [Params(0, 1, 10, 100, 1_000, 10_000)]
    public int AddRangeItemCount { get; set; } 

    [Benchmark(Baseline = true)]
    public void CurrentImplementation()
    {
        var list = new ChangeTrackingList<int>(items: _initialItemSetsByCount[InitialItemCount]);

        list.AddRange(_mutationsByItemCount[AddRangeItemCount].Items);
    }
    
    private static readonly Dictionary<int, ImmutableArray<int>>                            _initialItemSetsByCount;
    private static readonly Dictionary<int, AddRangeMutation<ChangeTrackingList<int>, int>> _mutationsByItemCount;
}
