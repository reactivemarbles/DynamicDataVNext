namespace DynamicDataVNext.Benchmarks.Ordered.ChangeTrackingList;

[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
public class InsertRange
{
    static InsertRange()
    {
        _initialItemSetsByCount = new();
        _mutationsByItemCounts  = new();

        var randomizer = new Randomizer(1234567);

        foreach (var initialItemCount in new[] { 0, 1, 10, 100, 1_000, 10_000 })
        {
            _initialItemSetsByCount.Add(
                key:    initialItemCount,
                value:  Enumerable.Repeat(randomizer, initialItemCount)
                    .Select(static randomizer => randomizer.Int())
                    .ToImmutableArray());

            foreach (var insertRangeItemCount in new[] { 0, 1, 10, 100, 1_000, 10_000 })
            {
                _mutationsByItemCounts.Add(
                    key:    (initialItemCount, insertRangeItemCount),
                    value:  new InsertRangeMutation<ChangeTrackingList<int>, int>()
                    {
                        Index = randomizer.Int(0, initialItemCount),
                        Items = Enumerable.Repeat(randomizer, insertRangeItemCount)
                            .Select(static randomizer => randomizer.Int())
                            .ToImmutableArray()
                    });
            }
        }
    }

    [Params(0, 1, 10, 100, 1_000, 10_000)]
    public int InitialItemCount { get; set; }
    
    [Params(0, 1, 10, 100, 1_000, 10_000)]
    public int InsertRangeItemCount { get; set; } 

    [Benchmark(Baseline = true)]
    public void CurrentImplementation()
    {
        var list = new ChangeTrackingList<int>(items: _initialItemSetsByCount[InitialItemCount]);

        var mutation = _mutationsByItemCounts[(InitialItemCount, InsertRangeItemCount)];

        list.InsertRange(
            index: mutation.Index,
            items: mutation.Items);
    }
    
    private static readonly Dictionary<int, ImmutableArray<int>>                                                                            _initialItemSetsByCount;
    private static readonly Dictionary<(int initialItemCount, int insertRangeItemCount), InsertRangeMutation<ChangeTrackingList<int>, int>> _mutationsByItemCounts;
}
