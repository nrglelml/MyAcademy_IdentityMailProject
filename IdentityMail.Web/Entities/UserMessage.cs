namespace IdentityMail.Web.Entities
{
    public class UserMessage
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
        public bool? IsImportant { get; set; }
        public AppUser Sender { get; set; }
        public int SenderId { get; set; }
        public AppUser Receiver { get; set; }
        public int ReceiverId { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool? IsArchived { get; set; }

        public int? ParentMessageId { get; set; }      
        public UserMessage? ParentMessage { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public bool IsReported { get; set; }
        public ICollection<MessageFolder> MessageFolders { get; set; }= new List<MessageFolder>();
    }
}
