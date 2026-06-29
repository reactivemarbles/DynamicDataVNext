using System;

namespace DynamicDataVNext;

public partial class ChangeSetBuilderBase<TChangeSet, TChange, TChangeType>
{
    /// <summary>
    /// A snapshot of state from a <see cref="ChangeSetBuilderBase{TChange,TChangeType,TChangeSet}"/> object, which may be used to restore that state after it has changed.
    /// </summary>
    public readonly struct Checkpoint
    {
        internal Checkpoint(ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> owner)
        {
            _owner = owner;
            
            _checkpointNonce                = _owner._checkpointNonce;
            _currentType                    = _owner._currentType;
            _firstResetAdditionIndex        = _owner._firstResetAdditionIndex;
            _isSourceEmpty                  = _owner._isSourceEmpty;
            _pendingChangeCount             = _owner._changes.Count;
            _pendingChangesHasNonRemovals   = _owner._changesHasNonRemovals;
        }
        
        /// <summary>
        /// Restores the state of the <see cref="ChangeSetBuilderBase{TChange,TChangeType,TChangeSet}"/> object that created this checkpoint to its prior state, at the time this checkpoint was created. 
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if the checkpoint has been invalidated by subsequent operations performed upon the builder.</exception>
        /// <remarks>
        /// Operations that invalidate checkpoints include <see cref="ChangeSetBuilderBase{TChange,TChangeType,TChangeSet}.BuildAndClear(bool)"/> and <see cref="ChangeSetBuilderBase{TChange,TChangeType,TChangeSet}.Clear()"/>.
        /// </remarks>
        public void Restore()
        {
            if (_owner._checkpointNonce != _checkpointNonce)
                throw new InvalidOperationException("Checkpoint is no longer valid");
            
            if (_owner._changes.Count == _pendingChangeCount)
                return;
            
            _owner._changes.RemoveRange(
                index:  _pendingChangeCount,
                length: _owner._changes.Count - _pendingChangeCount);

            _owner._currentType                     = _currentType;
            _owner._firstResetAdditionIndex         = _firstResetAdditionIndex;
            _owner._isSourceEmpty                   = _isSourceEmpty;
            _owner._changesHasNonRemovals    = _pendingChangesHasNonRemovals;
            
            unchecked { ++_owner._checkpointNonce; }
        }
        
        private readonly int                                                    _checkpointNonce;
        private readonly ChangeSetType                                          _currentType;
        private readonly int                                                    _firstResetAdditionIndex;
        private readonly bool                                                   _isSourceEmpty;
        private readonly ChangeSetBuilderBase<TChangeSet, TChange, TChangeType> _owner;
        private readonly int                                                    _pendingChangeCount;
        private readonly bool                                                   _pendingChangesHasNonRemovals;
    }
}
