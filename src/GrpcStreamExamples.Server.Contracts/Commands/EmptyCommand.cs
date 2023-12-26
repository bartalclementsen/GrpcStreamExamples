namespace GrpcStreamExamples.Server.Contracts.Commands;

[ProtoContract]
public record EmptyCommand : CommandRequest { }
