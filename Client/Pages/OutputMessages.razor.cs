using MailTask.Shared;
using MailTask.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MailTask.Client.Pages
{
    public partial class OutputMessages : ComponentBase
    {
        [Inject] public HttpClient Http { get; set; }
        private IEnumerable<MailMessageResponse> messages;
        protected override async Task OnInitializedAsync()
        {
            messages = await Http.GetFromJsonAsync<IEnumerable<MailMessageResponse>>(Routes.V1.Sent);
        }
    }
}