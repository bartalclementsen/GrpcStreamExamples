namespace GrpcStreamExamples.Server.Contracts;

[Service]
public interface IGreeterService
{
    // Request/Response Part
    Task<SayHelloResponse> SayHelloAsync(SayHelloRequest request, CallContext context = default);

    // Streaming Part
    Task<CommandResponse> CommandAsync(CommandRequest request, CallContext context = default);

    IAsyncEnumerable<StreamResponse> StreamAsync(StreamRequest streamRequest, CallContext context = default);
}
