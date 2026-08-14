namespace IdentityMail.Web.DTOs.AdminDtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
    }
}
