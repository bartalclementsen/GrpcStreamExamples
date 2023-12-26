using ProtoBuf.Grpc;

namespace GrpcStreamExamples.Server.Services;

public class GreeterService : ServiceBase, IGreeterService
{
    private readonly ILogger<GreeterService> _logger;
    private readonly IMediator _mediator;
    private readonly ListenersHandler _listenersHandler;

    public GreeterService(ILogger<GreeterService> logger, IMediator mediator, ListenersHandler listenersHandler)
    {
        _logger = logger;
        _mediator = mediator;
        _listenersHandler = listenersHandler;
    }

    public async Task<SayHelloResponse> SayHelloAsync(SayHelloRequest request, CallContext context = default)
    {
        SayHelloResponse response = await _mediator.Send(request);

        await _listenersHandler.SendToAllAsync(new EmptyStreamResponse());

        return response;
    }

    public IAsyncEnumerable<StreamResponse> StreamAsync(StreamRequest streamRequest, CallContext context = default)
    {
        return _listenersHandler.CreateListenerAsync(streamRequest, context);
    }

    public Task<CommandResponse> CommandAsync(CommandRequest request, CallContext context = default)
    {
        return _mediator.Send(request).AsTask();
    }
}
