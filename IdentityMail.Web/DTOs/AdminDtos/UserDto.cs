namespace IdentityMail.Web.DTOs.AdminDtos
{
   public class UserDto
    {
        public List<TopSender> TopSenders { get; set; } = new();
        public List<TopCategory> TopCategories { get; set; } = new();
        
    }
    public class TopSender
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int MessageCount { get; set; }
    }

    public class TopCategory
    {
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public int MessageCount { get; set; }
    }
}
