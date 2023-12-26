namespace GrpcStreamExamples.Server.Contracts.Commands;

[ProtoContract]
public class CommandResponse
{
    [ProtoMember(1)]
    public bool Success { get; set; }

    [ProtoMember(2)]
    public string? Error { get; set; }
}

