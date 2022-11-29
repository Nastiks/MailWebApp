using MailTask.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace MailTask.Server.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMailNotification(string sender, string recipientId)
        {
            await Clients.User(recipientId).SendAsync("ReceiveMailNotification", sender);
        }

        public async Task SendMailMessage(MailMessageResponse message, string recipientId)
        {
            await Clients.User(recipientId).SendAsync("ReceiveMailMessage", message);
        }
    }
}