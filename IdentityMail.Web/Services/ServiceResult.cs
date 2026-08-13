namespace IdentityMail.Web.Services
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int? MessageId { get; set; }

        public static ServiceResult Ok(int? messageId = null) =>
            new() { Success = true, MessageId = messageId };

        public static ServiceResult Fail(string error) =>
            new() { Success = false, ErrorMessage = error };
    }
}
