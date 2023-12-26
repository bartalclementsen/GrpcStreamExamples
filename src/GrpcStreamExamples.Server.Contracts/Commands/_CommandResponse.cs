namespace GrpcStreamExamples.Server.Contracts.Commands;

[ProtoContract]
public record CommandResponse
{
    [ProtoMember(1)]
    public bool Success { get; init; }

    [ProtoMember(2)]
    public string? Error { get; init; }
}

