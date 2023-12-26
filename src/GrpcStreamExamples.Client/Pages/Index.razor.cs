using Grpc.Core;
using GrpcStreamExamples.Client.Services;
using GrpcStreamExamples.Server.Contracts;
using GrpcStreamExamples.Server.Contracts.Commands;
using GrpcStreamExamples.Server.Contracts.Streaming;
using Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.ObjectModel;

namespace GrpcStreamExamples.Client.Pages;

public partial class Index
{
    private string? _message;

    private ObservableCollection<string> _messages = new ObservableCollection<string>();

    [Inject] private IGreeterService _greeterService { get; set; } = default!;

    [Inject] private IEventAggregator _eventAggregator { get; set; } = default!;

    private ISubscription? subscription;
    public Guid Guid = Guid.NewGuid();

    protected override async Task OnInitializedAsync()
    {
        subscription = _eventAggregator.Subscribe((SayHelloStreamResponse notification, CancellationToken ct) =>
        {
            _messages.Add("From SignalR Stream " + (notification?.Message ?? "EMPTY MESSAGE"));
            StateHasChanged();
            return Task.CompletedTask;
        });

        // StreamRequest should be from JWT
        var channel = _greeterService.StreamAsync(new StreamRequest
        {
            Id = 1,
            Name = "Bartal"
        });

        await foreach (var message in channel)
        {
            if (message is SayHelloStreamResponse sayHelloStreamResponse)
            {
                _messages.Add(("From Stream " + (sayHelloStreamResponse?.Message ?? "EMPTY MESSAGE")) + " Guid: " + Guid.ToString());
            }
            else
            {
                _messages.Add($"Stream received of type: {message?.GetType()}" + " Guid: " + Guid.ToString());
            }

            StateHasChanged();
        }

        await base.OnInitializedAsync();
    }

    private async Task OnSendButtonClicked(MouseEventArgs args)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_message))
                return;

            var messageResponse = await _greeterService.SayHelloAsync(new SayHelloRequest()
            {
                Name = _message
            });

            _messages.Add((messageResponse.Message ?? "EMPTY MESSAGE") + " Guid: " + Guid.ToString());
        }
        catch (RpcException ex)
        {
            _messages.Add(ex.ToString());
        }
    }

    private async Task OnSendAsCommandButtonClicked(MouseEventArgs args)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_message))
                return;

            var messageResponse = await _greeterService.CommandAsync(new SayHelloCommand
            {
                Name = _message
            });

            //var messageResponse = await _greeterService.CommandAsync(new CommandRequest
            //{
            //    Command = new SayHelloCommand
            //    {
            //        Name = _message
            //    }
            //});

            _messages.Add($"Success: {messageResponse?.Success}, Error: {messageResponse?.Error}. " + Guid.ToString());
        }
        catch (RpcException ex)
        {
            _messages.Add(ex.ToString());
        }
        catch(Exception ex) 
        {
            _messages.Add("!!BAD!! " + ex.ToString());
        }
    }
}
