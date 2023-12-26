namespace GrpcStreamExamples.Server.Handlers;

public class SayHelloRequestHandler : IRequestHandler<SayHelloRequest, SayHelloResponse>
{
    public ValueTask<SayHelloResponse> Handle(SayHelloRequest request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(new SayHelloResponse
        {
            Message = "Hello " + request.Name
        });
    }
}
