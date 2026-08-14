using IdentityMail.Web.DTOs.UserMessagesDtos;

namespace IdentityMail.Web.DTOs.AdminDtos
{
    public class ReportMessageDetail
    {

        public int MessageId { get; set; }

        public string ReportByUser { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Body{ get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
  
    }
}
