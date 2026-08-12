namespace IdentityMail.Web.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; } = "#6366F1";

        public int UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<UserMessage> Messages { get; set; }
    }
}
