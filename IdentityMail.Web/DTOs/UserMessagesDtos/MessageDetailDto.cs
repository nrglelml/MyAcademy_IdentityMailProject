namespace IdentityMail.Web.DTOs.UserMessagesDtos
{
    public class MessageThreadItem
    {
        public string SenderName { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public bool IsCurrentMessage { get; set; }
    }

    public class MessageDetailDto
    {
        public int MessageId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string? CategoryName { get; set; }

        // Görüntüleyen kişi gönderen mi? (okundu bilgisi için)
        public bool IsViewerSender { get; set; }
        public string OtherPartyEmail { get; set; } = string.Empty;

        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }

        // En eskisi başta, en yenisi (bu mesaj) sonda
        public List<MessageThreadItem> Thread { get; set; } = new();
    }
}
