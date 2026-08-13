namespace IdentityMail.Web.DTOs.UserMessagesDtos
{
    public class MessageListDto
    {
        public int MessageId { get; set; }
        public int MessageFolderId { get; set; }   // star/archive/delete işlemleri bu Id'ye gidecek

        public string SenderName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string BodyPreview { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }

        public bool IsRead { get; set; }
        public bool IsStarred { get; set; }
        public bool IsReported { get; set; }

        public string? CategoryName { get; set; }

    }
}
