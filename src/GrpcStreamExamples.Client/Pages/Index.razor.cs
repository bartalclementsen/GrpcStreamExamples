namespace GrpcStreamExamples.Client.Pages;

public partial class Index
{
    private string? _message;

    private readonly ObservableCollection<string> _messages = [];

    [Inject] private IGreeterService _greeterService { get; set; } = default!;

    [Inject] private IEventAggregator _eventAggregator { get; set; } = default!;

    private ISubscription? subscription;

    protected override async Task OnInitializedAsync()
    {
        subscription = _eventAggregator.Subscribe((SayHelloStreamResponse notification, CancellationToken ct) =>
        {
            _messages.Add("From Stream " + (notification?.Message ?? "EMPTY MESSAGE"));
            StateHasChanged();
            return Task.CompletedTask;
        });

        await base.OnInitializedAsync();
    }

    private async Task OnSendButtonClicked(MouseEventArgs args)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_message))
                return;

            SayHelloResponse messageResponse = await _greeterService.SayHelloAsync(new SayHelloRequest()
            {
                Name = _message
            });

            _messages.Add(messageResponse.Message ?? "EMPTY MESSAGE");
        }
        catch (Exception ex)
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

            CommandResponse messageResponse = await _greeterService.CommandAsync(new SayHelloCommand
            {
                Name = _message
            });

            _messages.Add($"Success: {messageResponse?.Success}, Error: {messageResponse?.Error}");
        }
        catch (Exception ex)
        {
            _messages.Add(ex.ToString());
        }
    }
}
