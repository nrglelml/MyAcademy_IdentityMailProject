using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.UserMessagesDtos;
using IdentityMail.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Services.MessageServices
{
    public class MessageService(AppDbContext _context) : IMessageService
    {
        public async Task<ServiceResult> SendMessageAsync(int senderId, ComposeMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ReceiverEmail))
                return ServiceResult.Fail("Alıcı e-postası zorunlu.");

            if (string.IsNullOrWhiteSpace(dto.Subject))
                return ServiceResult.Fail("Konu zorunlu.");

            if (string.IsNullOrWhiteSpace(dto.Body))
                return ServiceResult.Fail("Mesaj içeriği zorunlu.");

            var receiver = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.ReceiverEmail);

            if (receiver == null)
                return ServiceResult.Fail("Bu e-posta adresine kayıtlı bir kullanıcı bulunamadı.");
            if(!receiver.IsActive)
                return ServiceResult.Fail("Alıcı hesabı aktif değil.");
            if (receiver.Id == senderId)
                return ServiceResult.Fail("Kendinize mesaj gönderemezsiniz.");

            if (dto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                    .AnyAsync(c => c.Id == dto.CategoryId.Value);

                if (!categoryExists)
                    return ServiceResult.Fail("Geçersiz kategori.");
            }

            var message = new UserMessage
            {
                SenderId = senderId,
                ReceiverId = receiver.Id,
                Subject = dto.Subject,
                Body = dto.Body,
                SendDate = DateTime.UtcNow,
                IsRead = false,
                CategoryId = dto.CategoryId,
                ParentMessageId = dto.ParentMessageId
            };

            message.MessageFolders.Add(new MessageFolder
            {
                UserId = senderId,
                FolderType = FolderType.Sent,
                IsStarred = false,
                IsDeleted = false
            });

            message.MessageFolders.Add(new MessageFolder
            {
                UserId = receiver.Id,
                FolderType = FolderType.Inbox,
                IsStarred = false,
                IsDeleted = false
            });

            _context.UserMessages.Add(message);

            if (dto.ExistingDraftId.HasValue)
            {
                var draft = await _context.Drafts
                    .FirstOrDefaultAsync(d => d.Id == dto.ExistingDraftId.Value && d.SenderId == senderId);

                if (draft != null)
                {
                    _context.Drafts.Remove(draft);
                }
            }

            await _context.SaveChangesAsync();

            return ServiceResult.Ok(message.Id);
        }

        public async Task<ServiceResult> SaveDraftAsync(int senderId, ComposeMessageDto dto)
        {
            if (dto.ExistingDraftId.HasValue)
            {
                var existing = await _context.Drafts
                    .FirstOrDefaultAsync(d => d.Id == dto.ExistingDraftId.Value && d.SenderId == senderId);

                if (existing == null)
                    return ServiceResult.Fail("Taslak bulunamadı.");

                existing.ReceiverEmail = dto.ReceiverEmail;
                existing.Subject = dto.Subject ?? string.Empty;
                existing.Body = dto.Body ?? string.Empty;
                existing.CategoryId = dto.CategoryId;
                existing.ParentMessageId = dto.ParentMessageId;
                existing.LastEditedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return ServiceResult.Ok(existing.Id);
            }

            var draft = new Draft
            {
                SenderId = senderId,
                ReceiverEmail = dto.ReceiverEmail,
                Subject = dto.Subject ?? string.Empty,
                Body = dto.Body ?? string.Empty,
                CategoryId = dto.CategoryId,
                ParentMessageId = dto.ParentMessageId,
                LastEditedDate = DateTime.UtcNow
            };

            _context.Drafts.Add(draft);
            await _context.SaveChangesAsync();

            return ServiceResult.Ok(draft.Id);
        }
    }
}
