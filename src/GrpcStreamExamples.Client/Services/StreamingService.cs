using GrpcStreamExamples.Server.Contracts;
using GrpcStreamExamples.Server.Contracts.Streaming;
using Mediator;

namespace GrpcStreamExamples.Client.Services;

public interface IStreamingService
{
    void Start();
}

public class StreamingService : IStreamingService
{
    private readonly IGreeterService _greeterService;
    private IMediator _mediator;
    private Task? _streamListeningTask;

    public StreamingService(IGreeterService greeterService, IMediator mediator)
    {
        _greeterService = greeterService ?? throw new ArgumentNullException(nameof(greeterService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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


        // StreamRequest should be from JWT
        var channel = _greeterService.StreamAsync(new StreamRequest
        {
            Id = 1,
            Name = "Bartal"
        });

        await foreach (StreamResponse message in channel)
        {
            if (message is SayHelloStreamResponse sayHelloStreamResponse)
            {
                await _mediator.Publish(sayHelloStreamResponse);
            }
            else
            {
            }
        }
    }
}
