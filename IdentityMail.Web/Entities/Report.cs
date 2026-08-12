namespace IdentityMail.Web.Entities
{
    public class Report
    {
        public int Id { get; set; }

        public int MessageId { get; set; }
        public UserMessage Message { get; set; }

        public int ReportedByUserId { get; set; }
        public AppUser ReportedByUser { get; set; }

        public string Reason { get; set; }
        public DateTime ReportDate { get; set; }
        public ReportStatus Status { get; set; }   
    }

    public enum ReportStatus { Pending, Reviewed, Dismissed }
}
