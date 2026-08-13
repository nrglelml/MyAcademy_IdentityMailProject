namespace IdentityMail.Web.Entities
{
    public class Draft
    {
        public int Id { get; set; }

        public int SenderId { get; set; }
        public AppUser Sender { get; set; } = null!;

        public string? ReceiverEmail { get; set; }

        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? ParentMessageId { get; set; }
        public UserMessage? ParentMessage { get; set; }

        public DateTime LastEditedDate { get; set; }
    }
}
