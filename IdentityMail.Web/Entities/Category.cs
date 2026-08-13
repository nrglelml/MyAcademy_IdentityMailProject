namespace IdentityMail.Web.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; } = "#6366F1";
        public ICollection<UserMessage> Messages { get; set; } = new List<UserMessage>();
    }
}
