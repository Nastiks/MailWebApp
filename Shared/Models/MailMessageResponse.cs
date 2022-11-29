namespace MailTask.Shared.Models
{
    public class MailMessageResponse
    {
        public MailMessageResponse(MailMessage message) : this()
        {
            Timestamp = message.Timestamp;
            Sender = message.Sender?.Username;
            SenderId = message.SenderId;
            Recipient = message.Recipient?.Username;
            RecipientId = message.RecipientId;
            Body = message.Body;
            Subject = message.Subject;
        }

        public MailMessageResponse() { }

        public DateTime Timestamp { get; set; }
        public string SenderId { get; set; }
        public string Sender { get; set; }
        public string RecipientId { get; set; }
        public string Recipient { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}