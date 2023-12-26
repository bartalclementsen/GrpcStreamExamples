using Grpc.Core;

namespace GrpcStreamExamples.Server.Services;

public abstract class ServiceBase
{
    public RpcException Success()
    {
        return new RpcException(Status.DefaultSuccess);
    }

    public RpcException Ok(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.OK, detail, exception));
    }

    public RpcException Cancelled(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.Cancelled, detail, exception));
    }

    public RpcException Unknown(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.Unknown, detail, exception));
    }

    public RpcException InvalidArgument(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.InvalidArgument, detail, exception));
    }

    public RpcException DeadlineExceeded(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.DeadlineExceeded, detail, exception));
    }

    public RpcException NotFound(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.NotFound, detail, exception));
    }

    public RpcException AlreadyExists(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.AlreadyExists, detail, exception));
    }

    public RpcException PermissionDenied(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.PermissionDenied, detail, exception));
    }

    public RpcException Unauthenticated(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.Unauthenticated, detail, exception));
    }

    public RpcException ResourceExhausted(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.ResourceExhausted, detail, exception));
    }

    public RpcException FailedPrecondition(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.FailedPrecondition, detail, exception));
    }

    public RpcException Aborted(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.Aborted, detail, exception));
    }

    public RpcException OutOfRange(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.OutOfRange, detail, exception));
    }

    public RpcException Unimplemented(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.Unimplemented, detail, exception));
    }

    public RpcException Internal(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.Internal, detail, exception));
    }

    public RpcException Unavailable(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.Unavailable, detail, exception));
    }

    public RpcException DataLoss(string detail = "", Exception? exception = null)
    {
        throw new RpcException(new Status(StatusCode.DataLoss, detail, exception));
    }
}
