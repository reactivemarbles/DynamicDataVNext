using System;
using System.Reactive;
using System.Reactive.Linq;

namespace DynamicDataVNext;

public static partial class DistinctChangeStream
{
    /// <summary>
    /// Observes the number of items currently contained within the virtual collection represented by a given change stream.
    /// </summary>
    /// <param name="stream">The change stream to be observed.</param>
    /// <typeparam name="T">The type of the items in the stream.</typeparam>
    /// <returns>the number of items currently in the virtual collection.</returns>
    /// <remarks>
    /// The result of this operation is considered a state stream, rather than an event stream. It will always publish an initial notification, immediately upon subscription.
    /// </remarks>
    public static IObservable<int> Count<T>(this DistinctChangeStream<T> stream)
        => Observable.Create<int>(downstreamObserver =>
        {
            var hasInitialized  = false;
            var result          = 0;
            
            var subscription = stream.Source.SubscribeSafe(Observer.Create<DistinctChangeSet<T>>(
                onNext:         changeSet =>
                {
                    var priorResult = result;
                    
                    if (changeSet.Type is not ChangeSetType.Empty)
                        foreach (var change in changeSet.Changes)
                            result += change.Type switch
                            {
                                DistinctChangeType.Addition => 1,
                                DistinctChangeType.Removal  => -1,
                                _                           => 0        
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
