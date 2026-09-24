namespace DynamicDataVNext;

public static partial class KeyedChangeSet
{
    /// <summary>
    /// Applies the changes described within a <see cref="KeyedChangeSet{TKey, TItem}"/> to a given <see cref="ImmutableDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <param name="changeSet">The changes to be applied.</param>
    /// <param name="target">The target collection to which the changes are to be applied.</param>
    /// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
    /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
    /// <returns>A copy of <paramref name="target"/> that includes the given changes.</returns>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="target"/>.</exception>
    /// <exception cref="ArgumentException">Throws for malformed <paramref name="changeSet"/> values.</exception>
    public static ImmutableDictionary<TKey, TItem> ApplyTo<TKey, TItem>(
            this    KeyedChangeSet<TKey, TItem>         changeSet,
                    ImmutableDictionary<TKey, TItem>    target)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(target);
        
        switch (changeSet.Type)
        {
            case ChangeSetType.Clear:
                return target.Clear();

            case ChangeSetType.Empty:
                return target;

            case ChangeSetType.Reset:
                return ImmutableDictionary.CreateRange(
                    keyComparer:    target.KeyComparer,
                    items:          changeSet.AsReset().AdditionPairs);
                
            case ChangeSetType.Update:
                {
                    if (changeSet.Changes.Length is 1)
                        return changeSet.Changes[0].Type switch
                        {
                            KeyedChangeType.Addition    => target.Add(
                                key:    changeSet.Changes[0].Key,
                                value:  changeSet.Changes[0].AsAddition().Item),
                            KeyedChangeType.Removal     => target.Remove(changeSet.Changes[0].Key),
                            KeyedChangeType.Replacement => target.SetItem(
                                key:    changeSet.Changes[0].Key,
                                value:  changeSet.Changes[0].AsReplacement().NewItem),
                            _                           => target
                        };

                    var builder = target.ToBuilder();

                    foreach (var change in changeSet.Changes)
                        switch (change.Type)
                        {
                            case KeyedChangeType.Addition:
                                builder.Add(
                                    key:    change.Key,
                                    value:  change.AsAddition().Item);
                                break;
                            
                            case KeyedChangeType.Removal:
                                builder.Remove(change.Key);
                                break;

                            case KeyedChangeType.Replacement:
                                builder[change.Key] = change.AsReplacement().NewItem;
                                break;
                        }

                    return builder.ToImmutable();
                }

            default:
                throw new ArgumentException(
                    message:    $"Unsupported {nameof(KeyedChangeSet)} type {changeSet.Type}",
                    paramName:  nameof(changeSet));
        }
    }

    /// <summary>
    /// Applies the changes described within a <see cref="KeyedChangeSet{TKey, TItem}"/> to a given <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <param name="changeSet">The changes to be applied.</param>
    /// <param name="target">The target collection to which the changes are to be applied.</param>
    /// <typeparam name="TKey">The type of the key values in the collection.</typeparam>
    /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
    /// <typeparam name="TTarget">The type of the collection to be mutated.</typeparam>
    /// <returns>A copy of <paramref name="target"/> that includes the given changes.</returns>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="target"/>.</exception>
    /// <exception cref="ArgumentException">Throws for malformed <paramref name="changeSet"/> values.</exception>
    /// <remarks>
    /// Note that behavior is undefined when the change set is inconsistent with the target, I.E. when the upstream and downstream collections are out-of-sync. This method is only intended to be used to apply change sets in a valid sequence to a collection that intends to materialize them.
    /// </remarks>
    public static void ApplyTo<TKey, TItem, TTarget>(
            this    KeyedChangeSet<TKey, TItem> changeSet,
                    TTarget                     target)
        where TTarget : IDictionary<TKey, TItem>
        where TKey : notnull
    {
        if (target is null)
            throw new ArgumentNullException(nameof(target));

        switch (changeSet.Type)
        {
            case ChangeSetType.Clear:
                target.Clear();
                break;
            
            case ChangeSetType.Reset:
                {
                    var additionPairs = changeSet.AsReset().AdditionPairs;

                    if (target is IRangeAwareDictionary<TKey, TItem> rangeAwareDictionary)
                    {
                        rangeAwareDictionary.Reset(additionPairs);
                        return;
                    }
                    
                    target.Clear();

                    switch (target)
                    {
                        case Dictionary<TKey, TItem> dictionary:
                            dictionary.EnsureCapacity(additionPairs.Count);
                            break;
                            
                        case IExpandableCollection expandableCollection:
                            expandableCollection.EnsureCapacity(additionPairs.Count);
                            break;
                    }

                    foreach (var item in additionPairs)
                        target.Add(item);
                }
                break;
            
            case ChangeSetType.Update:
                var suspension = (target as IObservableCollection<KeyValuePair<TKey, TItem>>)?.SuspendNotifications();
                try
                {
                    foreach (var change in changeSet.Changes)
                        switch (change.Type)
                        {
                            case KeyedChangeType.Addition:
                                target.Add(change.AsAddition());
                                break;

                            case KeyedChangeType.Refreshment:
                                (target as IRefreshableDictionary<TKey>)?.Refresh(change.Key);
                                break;
                    
                            case KeyedChangeType.Removal:
                                target.Remove(change.Key);
                                break;

                            case KeyedChangeType.Replacement:
                                target[change.Key] = change.AsReplacement().NewItem;
                                break;
                        }
                }
                finally
                {
                    suspension?.Dispose();
                }
                break;
        }
    }
}
