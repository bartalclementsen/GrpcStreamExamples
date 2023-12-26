using GrpcStreamExamples.Server.Contracts;
using GrpcStreamExamples.Server.Contracts.Streaming;

namespace GrpcStreamExamples.Client.Services;

public interface IStreamingService
{
    void Start();
}

public class StreamingService : IStreamingService
{
    private readonly ILogger<StreamingService> _logger;
    private readonly IGreeterService _greeterService;
    private readonly IEventAggregator _eventAggregator;
    private Task? _streamListeningTask;


    public StreamingService(ILogger<StreamingService> logger, IGreeterService greeterService, IEventAggregator eventAggregator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _greeterService = greeterService ?? throw new ArgumentNullException(nameof(greeterService));
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
    }

    public void Start()
    {
        _streamListeningTask = StartListeningAsync();
    }

    private async Task StartListeningAsync()
    {
        // TODO: Should handle "not" listening when not logged in
        // TODO: Should handle "Start Listen" after login
        // TODO: Should handle "Stop Listen" when logged out
        // TODO: Should probably be able to handle new JWT after refresh.

        // StreamRequest id and name should be from JWT
        IAsyncEnumerable<StreamResponse> channel = _greeterService.StreamAsync(new StreamRequest
        {
            Id = 1,
            Name = "Bartal"
        });

        await foreach (StreamResponse streamResponse in channel)
        {
            if (streamResponse is SayHelloStreamResponse sayHelloStreamResponse)
            {
                await _eventAggregator.PublishAsync(sayHelloStreamResponse);
            }
            else
            {
                _logger.LogWarning("Unhandled StreamResponse. Type is {StreamResponseType}", streamResponse.GetType());
            }
        }
    }
}
