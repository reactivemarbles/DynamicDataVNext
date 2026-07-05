using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using AwesomeAssertions;

namespace DynamicDataVNext.Tests.Distinct;

public static class DistinctChangeStreamExtensions
{
    public static IDisposable RecordItems<T>(
        this    DistinctChangeStream<T>             stream,
        out     DistinctItemRecordingObserver<T>    observer,
                IScheduler?                         scheduler = null)
    {
        observer = new DistinctItemRecordingObserver<T>(
            scheduler:  scheduler ?? DefaultScheduler.Instance,
            comparer:   stream.Comparer);

        return stream.Source.Subscribe(observer);
    }

    public static DistinctChangeStream<T> ValidateChangeSets<T>(this DistinctChangeStream<T> stream)
        => stream with
        {
            // Using Raw observable and observer classes to bypass normal RX safeguards
            // This allows the operator to be combined with other operators that might be testing for things that the safeguards normally prevent.
            Source = RawAnonymousObservable.Create<DistinctChangeSet<T>>(observer =>
            {
                var items = new HashSet<T>(comparer: stream.Comparer);
                
                return stream.Source.SubscribeSafe(RawAnonymousObserver.Create<DistinctChangeSet<T>>(
                    onNext:         changeSet =>
                    {
                        try
                        {
                            changeSet.Should().BeValid();
                            
                            changeSet.Type.Should().NotBe(ChangeSetType.Empty, "empty changesets should be suppressed");
                            
                            switch (changeSet.Type)
                            {
                                case ChangeSetType.Clear:
                                    items.Should().NotBeEmpty("a clear should not be performed on an empty collection");
                                    break;
                            }

                            foreach (var change in changeSet.Changes)
                            {
                                switch (change.Type)
                                {
                                    case DistinctChangeType.Addition:
                                        items.Should().NotContain(change.Item, "item additions should not be performed for items already in a collection");
                                        items.Add(change.Item);
                                        break;
                                        
                                    case DistinctChangeType.Removal:
                                        items.Should().Contain(change.Item, "item removals should not be performed for items not in a collection");
                                        items.Remove(change.Item);
                                        break;
                                }
                            }

                            observer.OnNext(changeSet);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                        }
                    },
                    onError:        observer.OnError,
                    onCompleted:    observer.OnCompleted));
            })
        };
}
