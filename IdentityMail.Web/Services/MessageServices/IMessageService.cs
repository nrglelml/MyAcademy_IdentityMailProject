using IdentityMail.Web.DTOs.UserMessagesDtos;

namespace IdentityMail.Web.Services.MessageServices
{
    public interface IMessageService
    {
        Task<ServiceResult> SendMessageAsync(int senderId, ComposeMessageDto dto);
        Task<ServiceResult> SaveDraftAsync(int senderId, ComposeMessageDto dto);

        
        // Task<ServiceResult> ReplyAsync(int senderId, ComposeMessageDto dto);
        // Task<int> GetUnreadCountAsync(int userId);
        // Task<ServiceResult> MarkAsReadAsync(int messageId, int userId);
        // Task<ServiceResult> ToggleStarAsync(int messageFolderId, int userId);
        // Task<ServiceResult> MoveToTrashAsync(int messageFolderId, int userId);
        // Task<ServiceResult> RestoreAsync(int messageFolderId, int userId);
    }
}
