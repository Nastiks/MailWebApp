using MailTask.Shared;
using MailTask.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

namespace MailTask.Client.Pages
{
    public partial class InputMessages
    {
        [Inject] public HttpClient Http { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        public bool IsConnected => hub.State == HubConnectionState.Connected;

        private HashSet<MailMessageResponse> messages;
        private HubConnection hub;
        protected override async Task OnInitializedAsync()
        {
            var responce = await Http.GetFromJsonAsync<IEnumerable<MailMessageResponse>>(Routes.V1.Inbox);
            messages = new(responce);
            await ConfigureHubConnection();
        }

        private async Task ConfigureHubConnection()
        {
            hub = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/messageHub"))
                .Build();
            hub.On<MailMessageResponse>("ReceiveMailMessage", (message) =>
            {
                messages.Add(message);
                StateHasChanged();
            });
            await hub.StartAsync();
        }
    }
}