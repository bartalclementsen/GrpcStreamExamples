namespace GrpcStreamExamples.Server.Contracts;

[ProtoContract]
public class SayHelloResponse
{
    [ProtoMember(1)]
    public string? Message { get; set; }
}
