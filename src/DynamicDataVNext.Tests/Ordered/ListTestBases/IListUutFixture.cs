namespace DynamicDataVNext.Tests.Ordered.ListTestBases;

public interface IListUutFixture<out TUutFixture, out TUut>
        : IDisposable
    where TUutFixture : IListUutFixture<TUutFixture, TUut>
    where TUut : IList<string?>
{
    static abstract TUutFixture Create(OrderedItemOptions options = default);

    static abstract TUutFixture Create(
        int                 capacity,
        OrderedItemOptions  options     = default);

    static abstract TUutFixture Create(
        IEnumerable<string?>    items,
        OrderedItemOptions      options = default);
    
    TUut Uut { get; }
    
    OrderedItemOptions UutOptions { get; }

    void AssertItemsWereInserted(
        int                     insertionIndex,
        IReadOnlyList<string?>  insertedItems);

    void AssertItemsWereRemoved(IReadOnlyList<OrderedItem<string?>> removals);

    void AssertItemWasInserted(
        int     insertionIndex,
        string? insertedItem);

    void AssertItemWasMoved(
        int     oldIndex,
        int     newIndex,
        string? movedItem);

    void AssertItemWasRefreshed(
        int     refreshmentIndex,
        string? refreshedItem);

    void AssertItemWasRemoved(
        int     removalIndex,
        string? removedItem);

    void AssertItemWasReplaced(
        int     replacementIndex,
        string? replacedItem,
        string? replacementItem);

    void AssertUutDidNothing();
    
    void AssertUutWasCleared(IReadOnlyList<string?> removedItems);

    void AssertUutWasReset(
        IReadOnlyList<string?> removedItems,
        IReadOnlyList<string?> insertedItems);
}
