namespace MailTask.Shared.Models
{
    public class MailMessage
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }
        public string RecipientId { get; set; }
        public virtual User Recipient { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}