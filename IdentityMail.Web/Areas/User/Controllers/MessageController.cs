using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.UserMessagesDtos;
using IdentityMail.Web.Entities;
using IdentityMail.Web.Services.MessageServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.User.Controllers
{
    public class MessageController(UserManager<AppUser> _userManager, AppDbContext _context,IMessageService _messageService) : BaseUserController
    {
        public async Task<IActionResult> Inbox()
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var items = await _context.MessageFolders
                .Where(mf => mf.UserId == userId
                             && mf.FolderType == FolderType.Inbox
                             && !mf.IsDeleted)
                .Include(mf => mf.Message)
                    .ThenInclude(m => m.Sender)
                .Include(mf => mf.Message)
                    .ThenInclude(m => m.Category)
                .OrderByDescending(mf => mf.Message.SendDate)
                .Select(mf => new MessageListDto
                {
                    MessageId = mf.Message.Id,
                    MessageFolderId = mf.Id,
                    SenderName = mf.Message.Sender.FirstName + " " + mf.Message.Sender.LastName,
                    Subject = mf.Message.Subject,
                    BodyPreview = mf.Message.Body,
                    SendDate = mf.Message.SendDate,
                    IsRead = mf.Message.IsRead,
                    IsStarred = mf.IsStarred,
                    CategoryName = mf.Message.Category != null ? mf.Message.Category.Name : null
                })
                .ToListAsync();

            return View(items);
        }
        public async Task<IActionResult> Sendbox()
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var items = await _context.MessageFolders
                .Where(mf => mf.UserId == userId
                             && mf.FolderType == FolderType.Sent
                             && !mf.IsDeleted)
                .Include(mf => mf.Message)
                    .ThenInclude(m => m.Receiver)
                .Include(mf => mf.Message)
                    .ThenInclude(m => m.Category)
                .OrderByDescending(mf => mf.Message.SendDate)
                .Select(mf => new MessageListDto
                {
                    MessageId = mf.Message.Id,
                    MessageFolderId = mf.Id,
                    ReceiverName = mf.Message.Receiver.FirstName + " " + mf.Message.Receiver.LastName,
                    Subject = mf.Message.Subject,
                    BodyPreview = mf.Message.Body,
                    SendDate = mf.Message.SendDate,
                    IsRead = mf.Message.IsRead,
                    IsStarred = mf.IsStarred,
                    CategoryName = mf.Message.Category != null ? mf.Message.Category.Name : null
                })
                .ToListAsync();

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Compose(int? replyTo, int? draftId)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            await LoadCategoriesToViewBag();

            var dto = new ComposeMessageDto();

            if (draftId.HasValue)
            {
                var draft = await _context.Drafts
                    .FirstOrDefaultAsync(d => d.Id == draftId.Value && d.SenderId == userId);

                if (draft == null)
                {
                    TempData["ErrorMessage"] = "Taslak bulunamadı.";
                    return RedirectToAction("Drafts");
                }

                dto.ExistingDraftId = draft.Id;
                dto.ReceiverEmail = draft.ReceiverEmail;
                dto.Subject = draft.Subject;
                dto.Body = draft.Body;
                dto.CategoryId = draft.CategoryId;
                dto.ParentMessageId = draft.ParentMessageId;

                return View(dto);
            }

            if (replyTo.HasValue)
            {
                var original = await _context.UserMessages
                    .Include(m => m.Sender)
                    .FirstOrDefaultAsync(m => m.Id == replyTo.Value);

                if (original != null && original.Sender != null)
                {
                    dto.ReceiverEmail = original.Sender.Email;
                    dto.Subject = original.Subject.StartsWith("Re:") ? original.Subject : $"Re: {original.Subject}";
                    dto.ParentMessageId = original.Id;
                }
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Compose(ComposeMessageDto dto, string submitAction)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            if (submitAction == "draft")
            {
                var draftResult = await _messageService.SaveDraftAsync(userId, dto);

                if (!draftResult.Success)
                {
                    ModelState.AddModelError(string.Empty, draftResult.ErrorMessage!);
                    await LoadCategoriesToViewBag();
                    return View(dto);
                }

                TempData["SuccessMessage"] = "Taslak kaydedildi.";
                return RedirectToAction("Drafts");
            }

            var sendResult = await _messageService.SendMessageAsync(userId, dto);

            if (!sendResult.Success)
            {
                ModelState.AddModelError(string.Empty, sendResult.ErrorMessage!);
                await LoadCategoriesToViewBag();
                return View(dto);
            }

            TempData["SuccessMessage"] = "Mesaj gönderildi.";
            return RedirectToAction("Inbox");
        }

        private async Task LoadCategoriesToViewBag()
        {
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
