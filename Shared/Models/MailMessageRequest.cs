namespace MailTask.Shared.Models
{
    public class MailMessageRequest
    {
        public string RecipientId { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
