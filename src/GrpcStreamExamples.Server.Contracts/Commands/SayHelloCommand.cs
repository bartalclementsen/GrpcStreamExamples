namespace GrpcStreamExamples.Server.Contracts.Commands;

[ProtoContract]
public record SayHelloCommand : CommandRequest
{
    [ProtoMember(1)]
    public string? Name { get; init; }
}