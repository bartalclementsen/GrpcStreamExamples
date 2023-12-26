namespace GrpcStreamExamples.Server.Contracts.Streaming;

[ProtoContract]
public record SayHelloStreamResponse : StreamResponse
{
    [ProtoMember(1)]
    public string? Message { get; init; }
}
