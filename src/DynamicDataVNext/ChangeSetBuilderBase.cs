using System.Collections.Immutable;

namespace DynamicDataVNext;

/// <summary>
/// Describes an object capable of efficiently collecting individual changes, over time, to be assembled into a change set, with correctness guarantees.
/// </summary>
/// <typeparam name="TChange">The type of change values to be collected.</typeparam>
/// <typeparam name="TChangeSet">The type of changset values to be produced.</typeparam>
/// <remarks>
/// <para>In particular, when used properly, changesets produced by this class are guaranteed to be correct, with regard to automatically determining the correct <see cref="ChangeSetType"/> value, but not with regard to the individual sequence of changesets.</para>
/// <para>For example, a changeset of type <see cref="ChangeSetType.Clear"/> is guaranteed to only have removal changes, and should only be used when the consumer reports that the source collection has been emptied.</para>
/// <para>Likewise, a changeset of type <see cref="ChangeSetType.Reset"/> is guaranteed to start with removal changes for every item in the source collection, followed only by addition changes.</para>
/// <para>This builder class does not, however, guarantee that the changes given to it are actually valid, compared to previous changes.</para>
/// </remarks>
public abstract class ChangeSetBuilderBase<TChange, TChangeSet>
{
    /// <summary>
    /// Constructs a new, empty builder.
    /// </summary>
    internal ChangeSetBuilderBase()
        => _pendingChanges = ImmutableArray.CreateBuilder<TChange>();

    /// <summary>
    /// Constructs a new, empty builder, with a given initial <see cref="Capacity"/> value.
    /// </summary>
    /// <param name="initialCapacity">The initial value to use for <see cref="Capacity"/>.</param>
    internal ChangeSetBuilderBase(int initialCapacity)
        => _pendingChanges = ImmutableArray.CreateBuilder<TChange>(initialCapacity);

    /// <summary>
    /// The current size of the internal array, used to buffer changes until a call to <see cref="BuildAndClear(bool)"/>.
    /// </summary>
    /// <remarks>
    /// When building changesets where the number of changes is known ahead-of-time, setting this value (or calling <see cref="EnsureCapacity(int)"/>) to match the desired changeset size first can improve performance.
    /// </remarks>
    public int Capacity
    {
        get => _pendingChanges.Capacity;
        set => _pendingChanges.Capacity = value;
    }

    /// <summary>
    /// The number of changes currently buffered within the builder.
    /// </summary>
    public int Count
        => _pendingChanges.Count;

    /// <summary>
    /// Adds a given change to the buffer.
    /// </summary>
    /// <param name="change">The change to be added to the buffer.</param>
    public void AddChange(TChange change)
    {
        _pendingChanges.Add(change);
        if (!IsRemoval(change))
            _pendingChangesHasNonRemovals = true;

        _type = _type switch
        {
            ChangeSetType.Clear or ChangeSetType.Reset
                => IsAddition(change)
                    ? ChangeSetType.Reset
                    : ChangeSetType.Update,
            _ => ChangeSetType.Update
        };
    }

    /// <summary>
    /// Creates a changeset containing all of the currently-buffered changes, and clears the builder for future reuse.
    /// </summary>
    /// <param name="reuseBuffer">A flag indicating whether the builder should keep and reuse its internal buffer of changes, or relinquish ownership of the buffer and allocate a new one next time.</param>
    /// <returns>A changeset constructed from the changes buffered within the builder.</returns>
    /// <remarks>
    /// <para>The purpose of the <paramref name="reuseBuffer"/> flag is to allow for a memory-usage optimization for one-time-use scenarios, or scenarios where the changeset size is always known ahead-of-time.</para>
    /// <para>In either of these scnearios, setting <see cref="Capacity"/> to the exact number of changes to be added, before any of those changes are added, and then calling this method with <paramref name="reuseBuffer"/> as <see langword="false"/> allows the builder to avoid a copy allocation for the internal buffer of changes. The buffer is allocated once, populated, and then becomes the `<see cref="ImmutableArray{T}"/> that is published within the created changeset.</para>
    /// <para>Without this optimization, an extra allocation and copy operation is needed within this method, to duplicate the buffered changes into a new <see cref="ImmutableArray{T}"/>.</para>
    /// </remarks>
    public TChangeSet BuildAndClear(bool reuseBuffer = true)
    {
        if (_type is not ChangeSetType type)
            return Empty;

        var changes = reuseBuffer
            ? _pendingChanges.ToImmutable()
            : _pendingChanges.MoveToOrCreateImmutable();
        _pendingChanges.Clear();

        return CreateChangeSet(
            changes:    changes,
            type:       type);
    }

    /// <summary>
    /// Removes all buffered changes from the huilder.
    /// </summary>
    public void Clear()
        => _pendingChanges.Clear();

    /// <summary>
    /// Sets the value of <see cref="Capacity"/> to the given value, if it is less than the given value.
    /// </summary>
    /// <param name="capacity">The desired minimum value of <see cref="Capacity"/>.</param>
    public void EnsureCapacity(int capacity)
    {
        if (_pendingChanges.Capacity < capacity)
            _pendingChanges.Capacity = capacity;
    }

    /// <summary>
    /// Allows consumers to indicate that the source collection to which the buffered changes are applicable has been cleared, I.E. is empty, so that <see cref="BuildAndClear(bool)"/> can produce changesets of type <see cref="ChangeSetType.Clear"/> or <see cref="ChangeSetType.Reset"/>, if possible.
    /// </summary>
    public void OnSourceCleared()
    {
        if (!_pendingChangesHasNonRemovals)
            _type = ChangeSetType.Clear;
    }

    /// <summary>
    /// A copy of the "empty" changeset.
    /// </summary>
    protected abstract TChangeSet Empty { get; }

    /// <summary>
    /// Constructs a changeset from the given set of changes, and type value.
    /// </summary>
    /// <param name="changes">The set of changes to be embedded within the created changeset.</param>
    /// <param name="type">The type of changeset to be constructed.</param>
    /// <returns>A changeset constructed from <paramref name="changes"/> and <paramref name="type"/>.</returns>
    protected abstract TChangeSet CreateChangeSet(
        ImmutableArray<TChange> changes,
        ChangeSetType           type);

    /// <summary>
    /// Identifies a given change as an addition, or not.
    /// </summary>
    /// <param name="change">The change to be identified.</param>
    /// <returns>A flag indicating whether <paramref name="change"/> is an addition, or not.</returns>
    protected abstract bool IsAddition(TChange change);
    
    /// <summary>
    /// Identifies a given change as a removal, or not.
    /// </summary>
    /// <param name="change">The change to be identified.</param>
    /// <returns>A flag indicating whether <paramref name="change"/> is a removal, or not.</returns>
    protected abstract bool IsRemoval(TChange change);

    private readonly ImmutableArray<TChange>.Builder _pendingChanges;

    private bool            _pendingChangesHasNonRemovals;
    private ChangeSetType?  _type;
}
