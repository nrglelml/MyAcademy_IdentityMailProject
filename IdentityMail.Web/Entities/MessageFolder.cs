namespace IdentityMail.Web.Entities
{
    
        public enum FolderType
        {
            Inbox,
            Sent,
            Starred,   
            Trash
        }

        public class MessageFolder
        {
            public int Id { get; set; }

            public int MessageId { get; set; }
            public UserMessage Message { get; set; }

            public int UserId { get; set; }
            public AppUser User { get; set; }

            public FolderType FolderType { get; set; }
            public bool IsStarred { get; set; }    
            public bool IsDeleted { get; set; }   
        }
    
}
