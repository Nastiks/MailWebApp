using MailTask.Client.Services;
using MailTask.Shared;
using Microsoft.AspNetCore.Components;

namespace MailTask.Client.Shared
{
    public partial class LoginPartial
    {
        [Inject] public MailAuthenticationStateProvider HostAuthenticationStateProvider { get; set; }
        [Inject] public HttpClient Http { get; set; }
        [CascadingParameter] public MainLayout Parent { get; set; }

        private async Task LogoutAsync()
        {
            await Http.PostAsync(Routes.V1.Logout, null);
            HostAuthenticationStateProvider.RefreshState();
            await Parent.RefreshStateAsync();
        }
    }
}