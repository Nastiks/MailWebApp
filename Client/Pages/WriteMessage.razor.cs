using MailTask.Shared;
using MailTask.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

namespace MailTask.Client.Pages
{
    public partial class WriteMessage : ComponentBase
    {
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public HttpClient Http { get; set; }
        [Inject] public ISnackbar Bar { get; set; }

        private MailMessageRequest message = new();
        private HubConnection hub;
        private ClaimsPrincipal user;
        private bool isSending;

        public bool IsConnected => hub.State == HubConnectionState.Connected;
        protected override async Task OnInitializedAsync()
        {
            user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
            await ConfigureHubConnection();
        }

        private async Task ConfigureHubConnection()
        {
            hub = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/messageHub"))
                .Build();

            await hub.StartAsync();
        }

        private async Task<IEnumerable<User>> SearchUsersAsync(string username)
        {
            var responce = await Http.GetAsync($"{Routes.V1.UserSearch}?username={username}");
            if (responce.IsSuccessStatusCode)
            {
                return await responce.Content.ReadFromJsonAsync<IEnumerable<User>>();
            }

            return Array.Empty<User>();
        }

        private async Task SendMessageAsync()
        {
            isSending = true;
            var responce = await Http.PostAsJsonAsync(Routes.V1.Messages, message);
            if (IsConnected)
            {
                await hub.SendAsync("SendMailNotification", user.Identity.Name, message.RecipientId);
                if (responce.StatusCode == HttpStatusCode.OK)
                {
                    var mailMessage = await responce.Content.ReadFromJsonAsync<MailMessageResponse>();
                    await hub.SendAsync("SendMailMessage", mailMessage, message.RecipientId);
                }
            }
            message = new();
            isSending = false;
            Bar.Add("Message was sent successfully", Severity.Success);
        }
    }
}