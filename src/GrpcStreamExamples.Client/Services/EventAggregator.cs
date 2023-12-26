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
        Subscription<T> subscription = new(typeof(T), handler, this);
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

            if (subscriptions.Any() == false)
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

    private class Subscription<T> : ISubscription
    {
        /* ----------------------------------------------------------------------------  */
        /*                                  PROPERTIES                                   */
        /* ----------------------------------------------------------------------------  */
        private bool _disposed = false;

        public Guid Id { get; }

        public Type Type { get; }

        public Func<T, CancellationToken, Task> Handler { get; }

        private readonly IEventAggregator _eventAggregator;

        /* ----------------------------------------------------------------------------  */
        /*                                 CONSTRUCTORS                                  */
        /* ----------------------------------------------------------------------------  */
        public Subscription(Type type, Func<T, CancellationToken, Task> handler, IEventAggregator streamingService)
        {
            Id = Guid.NewGuid();
            Type = type;
            Handler = handler;
            _eventAggregator = streamingService;
        }

        /* ----------------------------------------------------------------------------  */
        /*                                PUBLIC METHODS                                 */
        /* ----------------------------------------------------------------------------  */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------------------  */
        /*                               PROTECTED METHODS                               */
        /* ----------------------------------------------------------------------------  */
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
