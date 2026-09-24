namespace DynamicDataVNext.Tests.Keyed;

public static class KeyedChangeStreamExtensions
{
    public static IDisposable RecordItems<TKey, TItem>(
            this    KeyedChangeStream<TKey, TItem>          stream,
            out     KeyedItemRecordingObserver<TKey, TItem> observer,
                    ISequencer?                             sequencer = null)
        where TKey : notnull
    {
        observer = new KeyedItemRecordingObserver<TKey, TItem>(
            sequencer:      sequencer ?? Sequencer.Default,
            keyComparer:    stream.KeyComparer);

        return stream.Source.Subscribe(observer);
    }

    public static KeyedChangeStream<TKey, TItem> ValidateChangeSets<TKey, TItem>(this KeyedChangeStream<TKey, TItem> stream)
            where TKey : notnull
        => stream with
        {
            // Using Raw observable and observer classes to bypass normal RX safeguards
            // This allows the operator to be combined with other operators that might be testing for things that the safeguards normally prevent.
            Source = RawAnonymousObservable.Create<KeyedChangeSet<TKey, TItem>>(observer =>
            {
                var items = new Dictionary<TKey, TItem>(comparer: stream.KeyComparer);
                
                return stream.Source.SubscribeSafe(RawAnonymousObserver.Create<KeyedChangeSet<TKey, TItem>>(
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
                                    case KeyedChangeType.Addition:
                                        var addition = change.AsAddition();
                                        items.Should().NotContain(addition, "item additions should not be performed for items already in a collection");
                                        items.Add(
                                            key:    addition.Key,
                                            value:  addition.Item);
                                        break;
                                        
                                    case KeyedChangeType.Refreshment:
                                        var refreshment = change.AsRefreshment();
                                        items.Should().Contain(refreshment, "item refreshments should not be performed for items not in a collection");
                                        break;

                                    case KeyedChangeType.Removal:
                                        var removal = change.AsRemoval();
                                        items.Should().Contain(removal, "item removals should not be performed for items not in a collection");
                                        items.Remove(removal.Key);
                                        break;

                                    case KeyedChangeType.Replacement:
                                        var replacement = change.AsReplacement();
                                        var oldItem = new KeyValuePair<TKey, TItem>(
                                            key:    replacement.Key,
                                            value:  replacement.OldItem);
                                        items.Should().Contain(oldItem, "item replacements should not be performed for items not in a collection");
                                        items[replacement.Key] = replacement.NewItem;
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
