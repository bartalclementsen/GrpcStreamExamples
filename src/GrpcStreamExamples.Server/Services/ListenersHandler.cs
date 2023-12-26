using ProtoBuf.Grpc;
using System.Threading.Channels;

namespace GrpcStreamExamples.Server.Services;

public class ListenersHandler
{
    private readonly List<Listener> _listeners = [];

    public IAsyncEnumerable<StreamResponse> CreateListenerAsync(StreamRequest streamRequest, CallContext context = default)
    {
        // Create the listener
        Listener listener = new(streamRequest, context);
        _listeners.Add(listener);

        return listener.GetStreamAsync();
    }

    public async Task SendToAllAsync(StreamResponse listenResponse, CancellationToken cancellationToken = default)
    {
        foreach (Listener listener in _listeners)
        {
            await listener.QueueMessageAsync(listenResponse, cancellationToken);
        }
    }

    public async Task SendToAsync(int id, StreamResponse listenResponse, CancellationToken cancellationToken = default)
    {
        Listener? listener = _listeners.FirstOrDefault(l => l.Id == id);

        if (listener == null)
        {
            throw new ArgumentException($"No listener with id {id} found", nameof(id));
        }

        await listener.QueueMessageAsync(listenResponse, cancellationToken);
    }

    public class Listener
    {
        public int Id => _streamRequest.Id;

        private readonly StreamRequest _streamRequest;
        private readonly CallContext _context;
        private readonly Channel<StreamResponse> _queue;

        public Listener(StreamRequest streamRequest, CallContext context)
        {
            _streamRequest = streamRequest;
            _context = context;
            _queue = Channel.CreateUnbounded<StreamResponse>();
        }

        public ValueTask QueueMessageAsync(StreamResponse listenResponse, CancellationToken cancellationToken = default)
        {
            return _queue.Writer.WriteAsync(listenResponse, cancellationToken);
        }

        public async IAsyncEnumerable<StreamResponse> GetStreamAsync()
        {
            await foreach (StreamResponse streamResponse in _queue.Reader.ReadAllAsync())
            {
                yield return streamResponse;
            }
        }
    }
}
