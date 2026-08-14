namespace IdentityMail.Web.DTOs.AdminDtos
{
    public class ReportedMessageListDto
    {
        public int MessageId { get; set; }

        public string ReportByUser{ get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
    }
}
