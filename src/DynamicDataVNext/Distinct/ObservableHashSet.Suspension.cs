namespace DynamicDataVNext;

public partial class ObservableHashSet<T>
{
    /// <summary>
    /// The value returned by <see cref="ObservableHashSet{T}.SuspendNotifications"/>, allowing consumers to control when notifications are resumed.
    /// </summary>
    public struct Suspension
        : IDisposable
    {
        internal Suspension(ObservableHashSet<T> owner)
            => _owner = owner;

        /// <summary>
        /// Instructs the <see cref="ObservableHashSet{T}"/> that created this to resume publishing notifications.
        /// </summary>
        public void Dispose()
        {
            if (_hasDisposed)
                return;
            _hasDisposed = true;
            
            if (_owner._hasDisposed)
                return;

            _owner._areNotificationsSuspended.OnNext(false);
            _owner.PublishNotificationsIfNeeded();
        }

        private readonly ObservableHashSet<T> _owner; 

        private bool _hasDisposed;
    }
}
