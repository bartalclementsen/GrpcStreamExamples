namespace GrpcStreamExamples.Server.Handlers;

public class SayHelloCommandHandler : IRequestHandler<SayHelloCommand, CommandResponse>
{
    private readonly ListenersHandler _listenersHandler;

    public SayHelloCommandHandler(ListenersHandler listenersHandler)
    {
        _listenersHandler = listenersHandler;
    }

    public async ValueTask<CommandResponse> Handle(SayHelloCommand request, CancellationToken cancellationToken)
    {
        bool success = false;
        string? error = null;

        try
        {
            await _listenersHandler.SendToAllAsync(new SayHelloStreamResponse()
            {
                Message = "Hello " + request.Name
            });

            success = true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
        }

        return new CommandResponse
        {
            Success = success,
            Error = error
        };
    }
}
