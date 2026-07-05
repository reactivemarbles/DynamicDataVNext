using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DynamicDataVNext;

public static partial class DistinctChangeSet
{
    /// <summary>
    /// Applies the changes described within a <see cref="DistinctChangeSet{T}"/> to a given <see cref="ImmutableHashSet{T}"/>.
    /// </summary>
    /// <param name="changeSet">The changes to be applied.</param>
    /// <param name="target">The target collection to which the changes are to be applied.</param>
    /// <typeparam name="T">The type of the items in <paramref name="target"/>.</typeparam>
    /// <returns>A copy of <paramref name="target"/> that includes the given changes.</returns>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="target"/>.</exception>
    /// <exception cref="ArgumentException">Throws for malformed <paramref name="changeSet"/> values.</exception>
    public static ImmutableHashSet<T> ApplyTo<T>(
        this    DistinctChangeSet<T>    changeSet,
                ImmutableHashSet<T>     target)
    {
        ArgumentNullException.ThrowIfNull(target);
        
        switch (changeSet.Type)
        {
            case ChangeSetType.Clear:
                return target.Clear();

            case ChangeSetType.Empty:
                return target;

            case ChangeSetType.Reset:
                return ImmutableHashSet.CreateRange(
                    equalityComparer:   target.KeyComparer,
                    items:              changeSet.AsReset().Additions);
                
            case ChangeSetType.Update:
                {
                    if (changeSet.Changes.Length is 1)
                        return changeSet.Changes[0].Type switch
                        {
                            DistinctChangeType.Addition => target.Add(changeSet.Changes[0].Item),
                            DistinctChangeType.Removal  => target.Remove(changeSet.Changes[0].Item),
                            _                           => target
                        };

                    var builder = target.ToBuilder();

                    foreach (var change in changeSet.Changes)
                        switch (change.Type)
                        {
                            case DistinctChangeType.Addition:
                                builder.Add(change.Item);
                                break;
                            
                            case DistinctChangeType.Removal:
                                builder.Remove(change.Item);
                                break;
                        }

                    return builder.ToImmutable();
                }

            default:
                throw new ArgumentException(
                    message:    $"Unsupported {nameof(DistinctChangeSet)} type {changeSet.Type}",
                    paramName:  nameof(changeSet));
        }
    }

    /// <summary>
    /// Applies the changes described within a <see cref="DistinctChangeSet{T}"/> to a given <see cref="ISet{T}"/>.
    /// </summary>
    /// <param name="changeSet">The changes to be applied.</param>
    /// <param name="target">The target collection to which the changes are to be applied.</param>
    /// <typeparam name="T">The type of the items in <paramref name="target"/>.</typeparam>
    /// <typeparam name="TTarget">The type of the collection to be mutated.</typeparam>
    /// <returns>A copy of <paramref name="target"/> that includes the given changes.</returns>
    /// <exception cref="ArgumentNullException">Throws for <paramref name="target"/>.</exception>
    /// <exception cref="ArgumentException">Throws for malformed <paramref name="changeSet"/> values.</exception>
    public static void ApplyTo<T, TTarget>(
            this    DistinctChangeSet<T>    changeSet,
                    TTarget                 target)
        where TTarget : ISet<T>
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
                    var additions = changeSet.AsReset().Additions;

                    if (target is IObservableSet<T> observableSet)
                    {
                        observableSet.Reset(additions);
                        return;
                    }

                    target.Clear();

                    switch (target)
                    {
                        case HashSet<T> hashSet:
                            hashSet.EnsureCapacity(additions.Count);
                            break;
                            
                        case IExpandableCollection expandableCollection:
                            expandableCollection.EnsureCapacity(additions.Count);
                            break;
                    }

                    foreach (var item in additions)
                        target.Add(item);
                }
                break;
            
            case ChangeSetType.Update:
                switch (target)
                {
                    case ChangeTrackingHashSet<T> changeTrackingHashSet:
                        foreach (var change in changeSet.Changes)
                            switch (change.Type)
                            {
                                case DistinctChangeType.Addition:
                                    changeTrackingHashSet.Add(change.Item);
                                    break;

                                case DistinctChangeType.Refreshment:
                                    changeTrackingHashSet.Refresh(change.Item);
                                    break;
                        
                                case DistinctChangeType.Removal:
                                    changeTrackingHashSet.Remove(change.Item);
                                    break;
                            }
                        break;
                
                    case IObservableSet<T> observableSet:
                        using (observableSet.SuspendNotifications())
                            foreach (var change in changeSet.Changes)
                                switch (change.Type)
                                {
                                    case DistinctChangeType.Addition:
                                        observableSet.Add(change.Item);
                                        break;

                                    case DistinctChangeType.Refreshment:
                                        observableSet.Refresh(change.Item);
                                        break;
                            
                                    case DistinctChangeType.Removal:
                                        observableSet.Remove(change.Item);
                                        break;
                                }
                        break;
                        
                    default:
                        foreach (var change in changeSet.Changes)
                            switch (change.Type)
                            {
                                case DistinctChangeType.Addition:
                                    target.Add(change.Item);
                                    break;

                                case DistinctChangeType.Removal:
                                    target.Remove(change.Item);
                                    break;
                            }
                        break;
                }
                break;
        }
    }
}
