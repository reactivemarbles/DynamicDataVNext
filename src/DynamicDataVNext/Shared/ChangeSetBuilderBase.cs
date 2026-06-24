using System;
using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Describes an object capable of efficiently collecting individual changes, over time, to be assembled into an optimal, valid change set.
/// </summary>
/// <typeparam name="TChange">The type of change values to be collected.</typeparam>
/// <typeparam name="TChangeType">An enum describing the types of change actions that can be represented by <typeparamref name="TChange"/>.</typeparam>
/// <typeparam name="TChangeSet">The type of changesets to be produced.</typeparam>
/// <remarks>
/// <para>In particular, when used properly, changesets produced by this class are guaranteed to be correct, with regard to automatically determining the correct <see cref="ChangeSetType"/> value, but not with regard to the individual sequence of changesets.</para>
/// <para>For example, the builder will produce a changeset of type <see cref="ChangeSetType.Clear"/> or <see cref="ChangeSetType.Reset"/> when the sequence of changes warrants it, but relies on the consumer to accurately report when the source collection is or is not empty.</para>
/// <para>This builder class does not, however, guarantee that the actual sequence of changes is valid, with respect to the source collection, as it does not receive a reference to the source collection.</para>
/// </remarks>
public abstract partial class ChangeSetBuilderBase<TChange, TChangeType, TChangeSet>
    where TChange : IChange<TChangeType>
    where TChangeType : Enum
{
    /// <summary>
    /// Constructs a new builder, with no collected changes.
    /// </summary>
    /// <param name="isSourceEmpty">A flag indicating whether the source collection that changes will refer to is currently empty.</param>
    protected ChangeSetBuilderBase(bool isSourceEmpty)
    {
        _changes        = new();
        _isSourceEmpty  = isSourceEmpty;
        _currentType    = ChangeSetType.Empty;
    }

    /// <inheritdoc cref="ChangeSetBuilderBase{TChange,TChangeType,TChangeSet}(bool)"/>
    /// <param name="isSourceEmpty">A flag indicating whether the source collection that changes will refer to is currently empty.</param>
    /// <param name="initialCapacity">The initial value to use for <see cref="Changes.Capacity"/>.</param>
    protected ChangeSetBuilderBase(
        int     initialCapacity,
        bool    isSourceEmpty)
    {
        _changes        = new(initialCapacity);
        _isSourceEmpty  = isSourceEmpty;
        _currentType    = ChangeSetType.Empty;
    }

    /// <summary>
    /// The collection of changes that have been buffered, but not yet captured into a changeset.
    /// </summary>
    public ChangeCollection Changes
        => _changes;
    
    /// <summary>
    /// The type of changeset that the sequence of changes in <see cref="Changes"/> currently represents.
    /// </summary>
    public ChangeSetType CurrentType
        => _currentType;
    
    /// <summary>
    /// A flag indicating whether the source collection is currently empty.
    /// </summary>
    public bool IsSourceEmpty
        => _isSourceEmpty;

    /// <summary>
    /// Adds a given change to <see cref="Changes"/>.
    /// </summary>
    /// <param name="change">The change to be added.</param>
    /// <param name="isSourceEmpty">A flag indicating whether the source collection is empty, after applying this change. I.E. whether this change removes the final item within the source collection.</param>
    /// <exception cref="ArgumentException">Throws when <paramref name="change"/> is invalid, or when the values of <paramref name="change"/> and <paramref name="isSourceEmpty"/> are incompatible.</exception>
    public void AddChange(
        TChange change,
        bool    isSourceEmpty = false)
    {
        var category = change.Category;

        _changesHasNonRemovals = category switch
        {
            ChangeCategory.None                             => throw new ArgumentException($"Change type {change.Type} not supported", nameof(change)),
            ChangeCategory.Removal when _isSourceEmpty      => throw new ArgumentException($"A change of category {change.Category} cannot be applied to an empty collection", nameof(change)),
            not ChangeCategory.Removal when isSourceEmpty   => throw new ArgumentException($"A collection cannot be emptied by a change of category {change.Category}", nameof(isSourceEmpty)),
            not ChangeCategory.Removal                      => true,
            _                                               => _changesHasNonRemovals
        };

        _changes.Add(change);

        // If the change set consists of only removals that leave the source collection empty, that's a clear
        if (isSourceEmpty && !_changesHasNonRemovals)
            _currentType = ChangeSetType.Clear;
        else
        {
            // Additions to an empty source collection, or following a clear, should count as a reset
            if (    (category is ChangeCategory.Addition)
                    &&  (       _isSourceEmpty
                                ||  (_currentType is ChangeSetType.Clear)))
            {
                _currentType                = ChangeSetType.Reset;
                _firstResetAdditionIndex    = _changes.Count - 1;
            }
            // As soon as we see a non-addition, it's no longer a reset, it's gotta just be an update
            else if ((category is not ChangeCategory.Addition) || (_currentType is not ChangeSetType.Reset))
                _currentType = ChangeSetType.Update;
        }

        _isSourceEmpty = isSourceEmpty;
    }

    /// <summary>
    /// Generates a new changeset, of type <see cref="CurrentType"/>, containing the sequence of changes in <see cref="Changes"/>, and resets the builder to an empty state. 
    /// </summary>
    /// <param name="willBuilderBeReused">A flag indicating whether the consumer intends to reuse this builder.</param>
    /// <returns>A changeset containing the captured state of the builder.</returns>
    /// <remarks>
    /// <para>The purpose of `<paramref name="willBuilderBeReused"/> is to allow consumers to skip buffer copying, when using a builder to build only a single changeset.</para>
    /// <para>That is, if the consumer intends to only build a single changeset, they can avoid an unnecessary buffer allocation by specifying an "initialCapacity" to the constructor, and then specifying a value of <see langword="false"/> for <paramref name="willBuilderBeReused"/>. When this happens, the underlying buffer for <see cref="Changes"/> will be pre-allocated, and then moved directly into the generated changeset, without being copied. Otherwise, a new buffer will be allocated when the changeset is generated, and the buffered changes will be copied into it.</para>
    /// <para>Also note that this operation invalidates any previously-created <see cref="Checkpoint"/>s.</para>
    /// </remarks>
    public TChangeSet BuildAndClear(bool willBuilderBeReused = true)
    {
        var changeSet = CreateChangeSet(
            changes:                    _changes.BuildImmutable(willBuilderBeReused),
            type:                       _currentType,
            firstResetAdditionIndex:    _firstResetAdditionIndex);
        
        Clear(_isSourceEmpty);

        return changeSet;
    }

    /// <summary>
    /// Resets the builder to an empty state, by clearing <see cref="Changes"/> and setting <see cref="CurrentType"/> to <see cref="ChangeSetType.Empty"/>.
    /// </summary>
    /// <param name="isSourceEmpty">A flag indicating whether the source collection to which future changes will refer is currently empty.</param>
    /// <remarks>
    /// <para>Note that this operation invalidates any previously-created <see cref="Checkpoint"/>s.</para>
    /// </remarks>
    public void Clear(bool isSourceEmpty)
    {
        _changes.Clear();
        
        _firstResetAdditionIndex    = 0;
        _isSourceEmpty              = isSourceEmpty;
        _changesHasNonRemovals      = false;
        _currentType                = ChangeSetType.Empty;

        // Changes removed from the builder cannot be recovered, so invalidate any outstanding checkpoints
        unchecked { ++_checkpointNonce; }
    }

    /// <summary>
    /// Creates a snapshot of the current state of the builder, which the consumer can elect to revert back to later.
    /// </summary>
    /// <returns>A value that can be used to later restore the current state of the builder.</returns>
    /// <remarks>
    /// This allows consumers to easily implement atomic multi-item operations, by allowing the tracking pending changes to be rolled back, if an error occurs in the middle of processing. 
    /// </remarks>
    public Checkpoint CreateCheckpoint()
        => new(this);

    /// <summary>
    /// Allows subclasses to define the proper logic for constructing a changeset of type <see cref="TChangeSet"/>.
    /// </summary>
    /// <param name="changes">The sequence of changes to be embedded in the changeset.</param>
    /// <param name="type">The type of changeset to be created</param>
    /// <param name="firstResetAdditionIndex">The index of the first change within <paramref name="changes"/> that is an <see cref="ChangeCategory.Addition"/>, when <paramref name="type"/> is <see cref="ChangeSetType.Reset"/>.</param>
    /// <returns>The constructed changeset.</returns>
    protected abstract TChangeSet CreateChangeSet(
        ImmutableArray<TChange> changes,
        ChangeSetType           type,
        int                     firstResetAdditionIndex);

    private readonly ChangeCollection _changes;

    private int             _checkpointNonce;
    private ChangeSetType   _currentType;
    private int             _firstResetAdditionIndex;
    private bool            _isSourceEmpty;
    private bool            _changesHasNonRemovals;
}
