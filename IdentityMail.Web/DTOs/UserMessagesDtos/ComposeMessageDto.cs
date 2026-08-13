namespace IdentityMail.Web.DTOs.UserMessagesDtos
{
    public class ComposeMessageDto
    {
        public string? ReceiverEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public int? CategoryId { get; set; }

        public int? ParentMessageId { get; set; }
        public int? ExistingDraftId { get; set; }
    }
}
