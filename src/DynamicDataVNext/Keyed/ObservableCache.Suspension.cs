namespace DynamicDataVNext;

public partial class ObservableCache<TKey, TItem>
{
    /// <summary>
    /// The value returned by <see cref="ObservableCache{TKey, TItem}.SuspendNotifications"/>, allowing consumers to control when notifications are resumed.
    /// </summary>
    public struct Suspension
        : IDisposable
    {
        internal Suspension(ObservableCache<TKey, TItem> owner)
            => _owner = owner;

        /// <summary>
        /// Instructs the <see cref="ObservableCache{TKey, TItem}"/> that created this to resume publishing notifications.
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

        private readonly ObservableCache<TKey, TItem> _owner; 

        private bool _hasDisposed;
    }
}
