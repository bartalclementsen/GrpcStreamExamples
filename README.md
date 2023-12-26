# GrpcStreamExamples

This is an example project showing how to use gRPC (Code First) with Blazor WASM.
The gRPC server is implemented CQRS'is using Mediator (Mediator.SourceGenerator nuget is used, but Mediatr will also work). 
There are implemented two strategies.

1. Request / Response
2. Streaming (i.e. Command request and streaming response)

## GrpcStreamExamples.Server.Contracts

This is the gRPC contracts use in the Code first implementation.
For the Request / Response strategy there are classes in the root namespace ``GrpcStreamExamples.Server.Contracts``
For the Streaming strategy all commands are in the namespace ``GrpcStreamExamples.Server.Contracts`` and all possible Streaming messages are in the namespace ``GrpcStreamExamples.Server.Streaming``

When adding a command you will need to create a new Command Class in ``GrpcStreamExamples.Server.Contracts.Commands`` that extends ``CommandRequest``.
Then you'll need to add a new ProtoInclude to the ``CommandRequest`` class with the new Command.

When adding a new StreamResponse you will need to create a new StreamResponse class in ``GrpcStreamExamples.Server.Contracts.Streaming`` that extends ``StreamResponse``.
Then you'll need to add a new ProtoInclude to the ``StreamResponse`` class with the new StreamResponse.

## GrpcStreamExamples.Server

This is the gRPC server that will handle all the request and streaming.

### GrpcStreamExamples.Server.Services.GreeterService
Is the implementation of the gRPC service. This should only "Re-send" the request to the Mediator.

### GrpcStreamExamples.Server.Services.ExceptionHelpers
Is a helper class to help "translate" exceptions to RpcExceptions.

### GrpcStreamExamples.Server.Services.ServiceBase
Is a base class used by gRPC service implementations. It will help to send "known" RpcExceptions for know states.

## GrpcStreamExamples.Client
The client
