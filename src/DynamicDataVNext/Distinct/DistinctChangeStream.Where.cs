namespace DynamicDataVNext;

public static partial class DistinctChangeStream
{
    /// <summary>
    /// Filters a virtual collection, represented by a given change stream, using a given static predicate. 
    /// </summary>
    /// <param name="stream">The stream whose virtual collection is to be filtered.</param>
    /// <param name="predicate">A static filtering predicate, to be used to determine whether items should be included or excluded by the filter.</param>
    /// <typeparam name="T">The type of items in the stream.</typeparam>
    /// <returns>A change stream representing the filtered virtual collection.</returns>
    /// <exception cref="ImmutableRefreshException">Throws if a <see cref="DistinctChangeType.Refreshment"/> change is received for an immutable item type.</exception>
    /// <remarks>
    /// It's important that the given stream accurately represents the mutability of its items, as this operator can make significant internal optimizations, when items are immutable. 
    /// </remarks>
    public static DistinctChangeStream<T> Where<T>(
        this    DistinctChangeStream<T> stream,
                Func<T, bool>           predicate)
    {
        return stream with
        {
            Source = Signal.Create<DistinctChangeSet<T>>(stream.Options.ItemsAreMutable
                ? SubscribeMutable
                : SubscribeImmutable)
        };
        
        IDisposable SubscribeImmutable(IObserver<DistinctChangeSet<T>> downstreamObserver)
        {
            var matchingItemCount = 0;
            var changeSetBuilder = new DistinctChangeSet<T>.Builder(isSourceEmpty: true);
            
            return stream.Source
                .Select(changeSet =>
                {
                    if (changeSet.Type is ChangeSetType.Empty)
                        return DistinctChangeSet.Empty<T>();
                    
                    changeSetBuilder.Changes.EnsureCapacity(changeSet.Changes.Length);
                    
                    foreach (var change in changeSet.Changes)
                    {
                        switch (change.Type)
                        {
                            case DistinctChangeType.Refreshment:
                                throw new ImmutableRefreshException();

                            case DistinctChangeType.None:
                                continue;
                        }

                        if (!predicate.Invoke(change.Item))
                            continue;
                        
                        matchingItemCount += (change.Type is DistinctChangeType.Addition)
                            ? 1
                            : -1;
                        
                        changeSetBuilder.AddChange(
                            change:         change,
                            isSourceEmpty:  matchingItemCount is 0);
                    }
                    
                    return changeSetBuilder.BuildAndClear(willBuilderBeReused: true);
                })
                .Where(static changeSet => changeSet.Type is not ChangeSetType.Empty)
                .SubscribeSafe(downstreamObserver);
        }

        IDisposable SubscribeMutable(IObserver<DistinctChangeSet<T>> downstreamObserver)
        {
            var downstreamItems = new ChangeTrackingHashSet<T>(
                comparer:   stream.Comparer,
                options:    stream.Options);

            return stream.Source
                .Select(changeSet =>
                {
                    switch (changeSet.Type)
                    {
                        case ChangeSetType.Clear:
                            downstreamItems.Clear();
                            break;
                        
                        case ChangeSetType.Reset:
                            {
                                downstreamItems.Clear();
                                foreach (var addition in changeSet.AsReset().Additions)
                                    if (predicate.Invoke(addition))
                                        downstreamItems.Add(addition);
                            }
                            break;
                        
                        case ChangeSetType.Update:
                            foreach (var change in changeSet.Changes)
                            {
                                switch (change.Type)
                                {
                                    case DistinctChangeType.Addition:
                                        if (predicate.Invoke(change.Item))
                                            downstreamItems.Add(change.Item);
                                        break;
                                    
                                    case DistinctChangeType.Refreshment:
                                        if (predicate.Invoke(change.Item))
                                            downstreamItems.Add(change.Item);
                                        else
                                            downstreamItems.Remove(change.Item);
                                        break;
                                    
                                    case DistinctChangeType.Removal:
                                        downstreamItems.Remove(change.Item);
                                        break;
                                }
                            }
                            break;
                    }
                    
                    return downstreamItems.BufferedChanges.CaptureAndClear();
                })
                .Where(static changeSet => changeSet.Type is not ChangeSetType.Empty)
                .SubscribeSafe(downstreamObserver);
        }
    }
}
