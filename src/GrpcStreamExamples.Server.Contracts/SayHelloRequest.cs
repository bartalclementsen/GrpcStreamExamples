namespace GrpcStreamExamples.Server.Contracts;

[ProtoContract]
public record SayHelloRequest : IRequest<SayHelloResponse>
{
    [ProtoMember(1)]
    public string? Name { get; init; }
}
