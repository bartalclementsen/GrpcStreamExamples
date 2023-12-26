namespace GrpcStreamExamples.Client.Services;

public interface ISubscription : IDisposable
{
    public Guid Id { get; }

    public Type Type { get; }
}

public interface IEventAggregator
{
    ISubscription Subscribe<T>(Func<T, CancellationToken, Task> handler);

    void Unsubscribe(ISubscription subscription);

    Task PublishAsync<T>(T notification, CancellationToken cancellationToken = default);
}

public class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, Dictionary<Guid, ISubscription>> _subscriptions = [];

    public ISubscription Subscribe<T>(Func<T, CancellationToken, Task> handler)
    {
        Subscription<T> subscription = new(handler, this);
        if (!_subscriptions.TryGetValue(typeof(T), out Dictionary<Guid, ISubscription>? subscriptions))
        {
            subscriptions = [];
            _subscriptions.Add(typeof(T), subscriptions);
        }

        subscriptions.Add(subscription.Id, subscription);
        return subscription;
    }

    public void Unsubscribe(ISubscription subscription)
    {
        if (_subscriptions.TryGetValue(subscription.Type, out Dictionary<Guid, ISubscription>? subscriptions))
        {
            subscriptions.Remove(subscription.Id);

            if (subscriptions.Count == 0)
            {
                _subscriptions.Remove(subscription.Type);
            }
        }
    }

    public async Task PublishAsync<T>(T notification, CancellationToken cancellationToken = default)
    {
        if (notification == null || !_subscriptions.TryGetValue(notification.GetType(), out Dictionary<Guid, ISubscription>? subscriptions))
        {
            return;
        }

        foreach (Subscription<T> subscription in subscriptions.Values.Cast<Subscription<T>>())
        {
            await subscription.Handler.Invoke(notification, cancellationToken);
        }
    }

    /* ----------------------------------------------------------------------------  */
    /*                                 INNER CLASSES                                 */
    /* ----------------------------------------------------------------------------  */
    private record Subscription<T> : ISubscription
    {
        private bool _disposed = false;

        public Guid Id { get; }

        public Type Type { get; }

        public Func<T, CancellationToken, Task> Handler { get; }

        private readonly IEventAggregator _eventAggregator;

        public Subscription(Func<T, CancellationToken, Task> handler, IEventAggregator streamingService)
        {
            Id = Guid.NewGuid();
            Type = typeof(T);
            Handler = handler;
            _eventAggregator = streamingService;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _eventAggregator.Unsubscribe(this);
            }

            _disposed = true;
        }
    }

}
