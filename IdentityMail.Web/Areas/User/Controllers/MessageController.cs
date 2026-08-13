using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.UserMessagesDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.User.Controllers
{
    public class MessageController(UserManager<AppUser> _userManager, AppDbContext _context) : BaseUserController
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
       
    }
}
