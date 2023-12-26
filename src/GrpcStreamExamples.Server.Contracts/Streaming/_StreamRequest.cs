namespace GrpcStreamExamples.Server.Contracts.Streaming;

// These should be taken from JWT instead of a Request message
[ProtoContract]
public record StreamRequest
{
    [ProtoMember(1)]
    public int Id { get; init; } = default!;

    [ProtoMember(2)]
    public string Name { get; init; } = default!;
}