namespace GrpcStreamExamples.Server.Contracts;

[ProtoContract]
public record SayHelloResponse
{
    [ProtoMember(1)]
    public string? Message { get; init; }
}
