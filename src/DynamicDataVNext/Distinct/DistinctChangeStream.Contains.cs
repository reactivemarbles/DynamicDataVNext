namespace DynamicDataVNext;

public static partial class DistinctChangeStream
{
    /// <summary>
    /// Observes whether a particular given item is currently contained within the virtual collection represented by a given change stream.
    /// </summary>
    /// <param name="stream">The change stream to be checked for containment of <paramref name="item"/>.</param>
    /// <param name="item">The item to check for, within <paramref name="stream"/>.</param>
    /// <typeparam name="T">The type of the items in the stream.</typeparam>
    /// <returns>A flag indicating whether the given virtual collection currently contains the given item.</returns>
    /// <exception cref="NotSupportedException">Throws if <paramref name="stream"/>.<see cref="DistinctChangeStream{T}.Options"/>.<see cref="DistinctItemOptions.ItemsAreMutable"/> is <see langword="true"/>.</exception>
    /// <remarks>
    /// The result of this operation is considered a state stream, rather than an event stream. It will always publish an initial notification, immediately upon subscription.
    /// </remarks>
    public static IObservable<bool> Contains<T>(
            this    DistinctChangeStream<T> stream,
                    T                       item)
        => stream.Options.ItemsAreMutable
            ? throw new NotSupportedException("Mutable items are not yet supported")
            : Signal.Create<bool>(downstreamObserver =>
            {
                var hasInitialized  = false;
                var result          = false;
                
                var subscription = stream.Source.SubscribeSafe(Witness.Create<DistinctChangeSet<T>>(
                    onNext:         changeSet =>
                    {
                        var priorResult = result;
                        
                        if (changeSet.Type is not ChangeSetType.Empty)
                            foreach (var change in changeSet.Changes)
                                if (stream.Comparer.Equals(change.Item, item))
                                    result = change.Type switch
                                    {
                                        DistinctChangeType.Addition => true,
                                        DistinctChangeType.Removal  => false,
                                        _                           => result
                                    };

                        if (!hasInitialized || (result != priorResult))
                            downstreamObserver.OnNext(result);
                        hasInitialized = true;
                    },
                    onError:        downstreamObserver.OnError,
                    onCompleted:    () =>
                    {
                        if (!hasInitialized)
                            downstreamObserver.OnNext(result);
                        hasInitialized = true;
                        
                        downstreamObserver.OnCompleted();
                    }));
                
                if (!hasInitialized)
                    downstreamObserver.OnNext(result);
                hasInitialized = true;
                
                return subscription;    
            });
}
