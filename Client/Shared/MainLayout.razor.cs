using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace MailTask.Client.Shared
{
    public partial class MainLayout
    {
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ISnackbar Bar { get; set; }
        private HubConnection hub;

        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();
            await ConfigureHubConnection();
        }

        private async Task ConfigureHubConnection()
        {
            hub = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/messageHub"))
                .Build();

            hub.On<string>("ReceiveMailNotification", (fromUser) =>
            {
                Bar.Add($"New message from {fromUser}", Severity.Info);
            });

            await hub.StartAsync();
        }

        public async Task RefreshStateAsync() => await OnInitializedAsync();
        public bool IsConnected => hub.State == HubConnectionState.Connected;
    }
}
