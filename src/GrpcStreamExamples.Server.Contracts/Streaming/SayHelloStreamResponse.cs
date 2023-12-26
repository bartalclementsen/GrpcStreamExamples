namespace GrpcStreamExamples.Server.Contracts.Streaming;

[ProtoContract]
public class SayHelloStreamResponse : StreamResponse
{
    [ProtoMember(1)]
    public string? Message { get; set; }
}
