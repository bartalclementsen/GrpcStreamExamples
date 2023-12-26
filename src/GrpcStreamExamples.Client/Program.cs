using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcStreamExamples.Client;
using GrpcStreamExamples.Client.Services;
using GrpcStreamExamples.Server.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProtoBuf.Grpc.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// gRPC Channel
builder.Services.AddSingleton(services =>
{
    // Get the service address from appsettings.json
    var config = services.GetRequiredService<IConfiguration>();
    var backendUrl = "https://localhost:5001"; //config["BackendUrl"];

    // If no address is set then fallback to the current webpage URL
    if (string.IsNullOrEmpty(backendUrl))
    {
        var navigationManager = services.GetRequiredService<NavigationManager>();
        backendUrl = navigationManager.BaseUri;
    }

    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());

    return GrpcChannel.ForAddress(
        backendUrl,
        new GrpcChannelOptions
        {
            HttpHandler = httpHandler,
            //CompressionProviders = ...,
            //Credentials = ...,
            //DisposeHttpClient = ...,
            //HttpClient = ...,
            //LoggerFactory = ...,
            //MaxReceiveMessageSize = ...,
            //MaxSendMessageSize = ...,
            //ThrowOperationCanceledOnCancellation = ...,
        });
});

builder.Services.AddTransient<IGreeterService>(services =>
{
    var grpcChannel = services.GetRequiredService<GrpcChannel>();
    return grpcChannel.CreateGrpcService<IGreeterService>();
});

builder.Services.AddSingleton<IEventAggregator, EventAggregator>();
builder.Services.AddSingleton<IStreamingService, StreamingService>();

var host = builder.Build();

IStreamingService streamingService = host.Services.GetRequiredService<IStreamingService>();
streamingService.Start();

await host.RunAsync();
