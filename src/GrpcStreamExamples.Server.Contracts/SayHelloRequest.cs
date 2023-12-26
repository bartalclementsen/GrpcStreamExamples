namespace GrpcStreamExamples.Server.Contracts;

[ProtoContract]
public class SayHelloRequest : IRequest<SayHelloResponse>
{
    [ProtoMember(1)]
    public string? Name { get; set; }
}
