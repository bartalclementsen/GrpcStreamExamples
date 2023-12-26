namespace GrpcStreamExamples.Server.Contracts.Streaming;

[ProtoContract]
[ProtoInclude(1, typeof(EmptyStreamResponse))]
[ProtoInclude(2, typeof(SayHelloStreamResponse))]
public abstract class StreamResponse { }
