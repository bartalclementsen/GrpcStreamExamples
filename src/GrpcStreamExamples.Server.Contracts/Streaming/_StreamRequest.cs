namespace GrpcStreamExamples.Server.Contracts.Streaming;

// These should be taken from JWT instead of a Request message
[ProtoContract]
public class StreamRequest
{
    [ProtoMember(1)]
    public int Id { get; set; } = default!;

    [ProtoMember(2)]
    public string Name { get; set; } = default!;
}