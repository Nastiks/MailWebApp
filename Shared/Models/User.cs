using System.Text.Json.Serialization;

namespace MailTask.Shared.Models
{
    public class User
    {
        public User()
        {
            Inbox = new HashSet<MailMessage>();
            Sent = new HashSet<MailMessage>();
        }

        public string Id { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public virtual ICollection<MailMessage> Inbox { get; set; }
        [JsonIgnore]
        public virtual ICollection<MailMessage> Sent { get; set; }
    }
}