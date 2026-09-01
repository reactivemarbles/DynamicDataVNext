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
public abstract partial class ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>
    where TChangeSet : struct, IChangeSet<TChange, TChangeType>
    where TChange : struct, IChange<TChangeType>
    where TChangeType : Enum
{
    /// <summary>
    /// Constructs a new builder, with no collected changes.
    /// </summary>
    /// <param name="sourceCount">The initial value to use for <see cref="SourceCount"/>.</param>
    protected ChangeSetBuilderBase(int sourceCount)
    {
        _changes        = new();
        _currentType    = ChangeSetType.Empty;
        _sourceCount    = sourceCount;
    }

    /// <inheritdoc cref="ChangeSetBuilderBase{TChange,TChangeType,TChangeSet}(int)"/>
    /// <param name="initialCapacity">The initial value to use for <see cref="Changes.Capacity"/>.</param>
    protected ChangeSetBuilderBase(
        int initialCapacity,
        int sourceCount)
    {
        _changes        = new(initialCapacity);
        _currentType    = ChangeSetType.Empty;
        _sourceCount    = sourceCount;
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
    /// The current value of <see cref="IReadOnlyCollection{T}.Count"/>, for the collection whose changes are being tracked.
    /// </summary>
    /// <remarks>
    /// Used to help track when specialized changeset types, such as <see cref="ChangeSetType.Reset"/> or <see cref="ChangeSetType.Clear"/> can be generated.
    /// </remarks>
    public int SourceCount
        => _sourceCount;

    /// <summary>
    /// Adds a given change to <see cref="Changes"/>.
    /// </summary>
    /// <param name="change">The change to be added.</param>
    /// <exception cref="ArgumentException">Throws when <paramref name="change"/> is invalid or inconsistent with the current state of the source collection.</exception>
    public void AddChange(TChange change)
    {
        var category = change.Category;

        switch (category)
        {
            case ChangeCategory.Addition:
                ++_sourceCount;
                _changesHasNonRemovals = true;
                break;

            case ChangeCategory.Removal:
                if (_sourceCount is 0)
                    throw new ArgumentException($"A change of category {change.Category} cannot be applied to an empty collection", nameof(change));
                --_sourceCount;
                break;

            case ChangeCategory.Other:
                _changesHasNonRemovals = true;
                break;

            default:
                throw new ArgumentException($"Change type {change.Type} not supported", nameof(change));
        }

        _changes.Add(change);

        _currentType = OnChangeAdded(change);

        // If this is the first addition within a Reset, record it. 
        if (        (category is ChangeCategory.Addition)
                &&  (_currentType is ChangeSetType.Reset)
                &&  (       (_changes.Count is 1)
                        ||  (_changes[^2].Category is not ChangeCategory.Addition)))
            _firstResetAdditionIndex = _changes.Count - 1;
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
        
        Clear(sourceCount: _sourceCount);

        return changeSet;
    }

    /// <summary>
    /// Resets the builder to an empty state, by clearing <see cref="Changes"/>, setting <see cref="CurrentType"/> to <see cref="ChangeSetType.Empty"/>, and resetting <see cref="SourceCount"/>.
    /// </summary>
    /// <param name="sourceCount">The value to which <see cref="SourceCount"/> is to be reset.</param>
    /// <remarks>
    /// <para>Note that this operation invalidates any previously-created <see cref="Checkpoint"/>s.</para>
    /// </remarks>
    public void Clear(int sourceCount)
    {
        _changes.Clear();
        
        _firstResetAdditionIndex    = 0;
        _sourceCount                = sourceCount;
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

    /// <summary>
    /// A handler to be invoked after a new change has been added to <see cref="Changes"/>.
    /// </summary>
    /// <param name="change">The change that was added.</param>
    /// <returns>The new value to use for <see cref="CurrentType"/>.</returns>
    /// <remarks>
    /// Allows consumers to inject custom logic for identifying special changeset types, such as <see cref="ChangeSetType.Reset"/> and <see cref="ChangeSetType.Clear"/>.
    /// </remarks>
    protected virtual ChangeSetType OnChangeAdded(TChange change)
    {
        var category = change.Category;

        // If the change set consists of only removals and leaves the source collection empty, that's a clear
        if ((_sourceCount is 0) && !_changesHasNonRemovals)
            return ChangeSetType.Clear;

        // Additions to an empty source collection, or following a clear, should count as a reset
        if (        (category is ChangeCategory.Addition)
                &&  (       (       (_currentType is ChangeSetType.Empty)
                                &&  (_sourceCount is 1))
                        ||  (_currentType is ChangeSetType.Clear)))
            return ChangeSetType.Reset;

        // If the changes currently represent a reset, and then we see a non-addition, it's no longer a reset, it's gotta just be an update.
        if (        (_currentType is not ChangeSetType.Reset)
                ||  (category is not ChangeCategory.Addition))
            return ChangeSetType.Update;

        // Otherwise, no change. The type continues to be either a Reset or an Update.
        return _currentType;
    }

    private readonly ChangeCollection _changes;

    private bool            _changesHasNonRemovals;
    private int             _checkpointNonce;
    private ChangeSetType   _currentType;
    private int             _firstResetAdditionIndex;
    private int             _sourceCount;
}
