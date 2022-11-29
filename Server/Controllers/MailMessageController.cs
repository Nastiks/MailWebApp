using MailTask.Server.Data;
using MailTask.Shared;
using MailTask.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MailTask.Server.Controllers
{
    public class MailMessageController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private User dbUser;
        internal User DbUser => GetUser();


        private User GetUser()
        {
            if (dbUser == null)
            {
                string userId = User.Claims.First(t => t.Type == ClaimTypes.NameIdentifier).Value;
                dbUser = dbContext.Users.First(x => x.Id == userId);
            }
            return dbUser;
        }

        public MailMessageController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpPost(Routes.V1.Messages)]
        public IActionResult CreateMessage([FromBody][Required] MailMessageRequest message)
        {
            MailMessage newMessage = new()
            {
                Body = message.Body,
                Id = default,
                RecipientId = message.RecipientId,
                Recipient = dbContext.Users.FirstOrDefault(x => x.Id == message.RecipientId),
                Sender = DbUser,
                SenderId = DbUser.Id,
                Timestamp = DateTime.UtcNow,
                Subject = message.Subject
            };
            dbContext.MailMessages.Add(newMessage);
            dbContext.SaveChanges();
            return Ok(new MailMessageResponse(newMessage));
        }

        [Authorize]
        [HttpGet(Routes.V1.Inbox)]
        public IActionResult GetInbox()
        {
            var messages = DbUser.Inbox.Any() ?
                DbUser.Inbox.Select(x => new MailMessageResponse(x)) :
                Array.Empty<MailMessageResponse>();
            return Ok(messages);
        }

        [Authorize]
        [HttpGet(Routes.V1.Sent)]
        public IActionResult GetSentMessages()
        {
            var messages = DbUser.Sent.Any() ?
                DbUser.Sent.Select(x => new MailMessageResponse(x)) :
                Array.Empty<MailMessageResponse>();
            return Ok(messages);
        }
    }
}