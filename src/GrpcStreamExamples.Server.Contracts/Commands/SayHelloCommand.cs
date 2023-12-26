namespace GrpcStreamExamples.Server.Contracts.Commands;

[ProtoContract]
public class SayHelloCommand : CommandRequest
{
    [ProtoMember(1)]
    public string? Name { get; set; }
}