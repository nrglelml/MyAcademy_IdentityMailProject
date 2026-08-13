namespace IdentityMail.Web.DTOs.DraftDtos
{
    public class DraftListDto
    {
        public int DraftId { get; set; }

        // Kayıtlı kullanıcıya denk geliyorsa adı, gelmiyorsa/boşsa e-posta ya da null.
        public string? RecipientDisplay { get; set; }

        public string Subject { get; set; } = string.Empty;
        public string BodyPreview { get; set; } = string.Empty;
        public DateTime LastEditedDate { get; set; }
    }
}
