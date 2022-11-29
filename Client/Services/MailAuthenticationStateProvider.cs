using MailTask.Shared;
using MailTask.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace MailTask.Client.Services
{
    public class MailAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        public MailAuthenticationStateProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var responce = (await httpClient.GetAsync(Routes.V1.CurrentUser))!;
            ClaimsIdentity claimsIdentity = new();
            if (!responce.IsSuccessStatusCode)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
            var user = await responce.Content.ReadFromJsonAsync<User>();
            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };
                claimsIdentity = new ClaimsIdentity(claims, "MailUser");
            }
            return new AuthenticationState(new ClaimsPrincipal(claimsIdentity));
        }

        public void RefreshState()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}