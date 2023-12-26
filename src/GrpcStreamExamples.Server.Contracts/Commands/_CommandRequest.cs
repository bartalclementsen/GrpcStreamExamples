namespace GrpcStreamExamples.Server.Contracts.Commands;

[ProtoContract]
[ProtoInclude(1, typeof(EmptyCommand))]
[ProtoInclude(2, typeof(SayHelloCommand))]
public abstract class CommandRequest : IRequest<CommandResponse>
{ }
