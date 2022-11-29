using MailTask.Client.Services;
using MailTask.Client.Shared;
using MailTask.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MailTask.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] public HttpClient Http { get; set; }
        [Inject] private MailAuthenticationStateProvider HostAuthenticationStateProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] public MainLayout Parent { get; set; }

        private string username;
        private async Task LoginAsync()
        {
            await Http.PostAsJsonAsync(Routes.V1.Login, username);
            HostAuthenticationStateProvider.RefreshState();
            await Parent.RefreshStateAsync();
        }
    }
}