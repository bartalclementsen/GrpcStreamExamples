using Grpc.Core;
using GrpcStreamExamples.Server.Contracts;
using GrpcStreamExamples.Server.Contracts.Commands;
using GrpcStreamExamples.Server.Contracts.Streaming;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.ObjectModel;

namespace GrpcStreamExamples.Client.Pages;

public partial class Index
{
    private string? _message;

    private ObservableCollection<string> _messages = new ObservableCollection<string>();

    [Inject] private IGreeterService _greeterService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        // StreamRequest should be from JWT
        //var channel = _greeterService.StreamAsync(new Server.Contracts.Streaming.StreamRequest
        //{
        //    Id = 1,
        //    Name = "Bartal"
        //});
        //await foreach (var message in channel)
        //{
        //    if(message.StreamBody is SayHelloStreamBody sayHelloStreamBody)
        //    {
        //        _messages.Add("From Stream " + (sayHelloStreamBody?.Message ?? "EMPTY MESSAGE"));
        //    } 
        //    else
        //    {
        //        _messages.Add($"Stream received of type: {message?.StreamBody?.GetType()}");
        //    }
            
        //    StateHasChanged();
        //}


        var channel = _greeterService.StreamAsync(new Server.Contracts.Streaming.StreamRequest
        {
            Id = 1,
            Name = "Bartal"
        });
        await foreach (var message in channel)
        {
            if (message is SayHelloStreamResponse sayHelloStreamBody)
            {
                _messages.Add("From Stream " + (sayHelloStreamBody?.Message ?? "EMPTY MESSAGE"));
            }
            else
            {
                _messages.Add($"Stream received of type: {message?.GetType()}");
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

            _messages.Add(messageResponse.Message ?? "EMPTY MESSAGE");
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

            _messages.Add($"Success: {messageResponse?.Success}, Error: {messageResponse?.Error}");
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
