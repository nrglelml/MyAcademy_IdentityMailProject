using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.UserMessagesDtos;
using IdentityMail.Web.Entities;
using IdentityMail.Web.Services.MessageServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace IdentityMail.Web.Areas.User.Controllers
{
    public class MessageController(UserManager<AppUser> _userManager, AppDbContext _context,IMessageService _messageService) : BaseUserController
    {
        public async Task<IActionResult> Inbox(
            string? sender, string? subject, DateTime? dateFrom, DateTime? dateTo,
            int? categoryId, bool? unreadOnly, bool? starredOnly,
            string sort = "desc", int page = 1)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);
            const int pageSize = 20;

            var query = _context.MessageFolders
                .Where(mf => mf.UserId == userId && mf.FolderType == FolderType.Inbox && !mf.IsDeleted)
                .Include(mf => mf.Message).ThenInclude(m => m.Sender)
                .Include(mf => mf.Message).ThenInclude(m => m.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(sender))
                query = query.Where(mf => (mf.Message.Sender.FirstName + " " + mf.Message.Sender.LastName).Contains(sender));

            if (!string.IsNullOrWhiteSpace(subject))
                query = query.Where(mf => mf.Message.Subject.Contains(subject));

            if (dateFrom.HasValue)
                query = query.Where(mf => mf.Message.SendDate >= dateFrom.Value.Date);

            if (dateTo.HasValue)
            {
                var inclusiveEnd = dateTo.Value.Date.AddDays(1); 
                query = query.Where(mf => mf.Message.SendDate < inclusiveEnd);
            }

            if (categoryId.HasValue)
                query = query.Where(mf => mf.Message.CategoryId == categoryId.Value);

            if (unreadOnly == true)
                query = query.Where(mf => !mf.Message.IsRead);

            if (starredOnly == true)
                query = query.Where(mf => mf.IsStarred);

            query = sort == "asc"
                ? query.OrderBy(mf => mf.Message.SendDate)
                : query.OrderByDescending(mf => mf.Message.SendDate);

            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            page = Math.Max(1, Math.Min(page, totalPages));

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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
                    IsReported = mf.Message.IsReported,
                    CategoryName = mf.Message.Category != null ? mf.Message.Category.Name : null
                })
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            ViewBag.FilterSender = sender;
            ViewBag.FilterSubject = subject;
            ViewBag.FilterDateFrom = dateFrom?.ToString("yyyy-MM-dd");
            ViewBag.FilterDateTo = dateTo?.ToString("yyyy-MM-dd");
            ViewBag.FilterCategoryId = categoryId;
            ViewBag.FilterUnreadOnly = unreadOnly == true;
            ViewBag.FilterStarredOnly = starredOnly == true;
            ViewBag.FilterSort = sort;

            return View(items);
        }
        public async Task<IActionResult> Sendbox(
            string? receiver, string? subject, DateTime? dateFrom, DateTime? dateTo,
            int? categoryId, bool? unreadOnly, bool? starredOnly,
            string sort = "desc", int page = 1)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);
            const int pageSize = 20;

            var query = _context.MessageFolders
                .Where(mf => mf.UserId == userId && mf.FolderType == FolderType.Sent && !mf.IsDeleted)
                .Include(mf => mf.Message).ThenInclude(m => m.Receiver)
                .Include(mf => mf.Message).ThenInclude(m => m.Category)
                .AsQueryable();

            // ---- Filtreler ----
            if (!string.IsNullOrWhiteSpace(receiver))
                query = query.Where(mf => (mf.Message.Receiver.FirstName + " " + mf.Message.Receiver.LastName).Contains(receiver));

            if (!string.IsNullOrWhiteSpace(subject))
                query = query.Where(mf => mf.Message.Subject.Contains(subject));

            if (dateFrom.HasValue)
                query = query.Where(mf => mf.Message.SendDate >= dateFrom.Value.Date);

            if (dateTo.HasValue)
            {
                var inclusiveEnd = dateTo.Value.Date.AddDays(1);
                query = query.Where(mf => mf.Message.SendDate < inclusiveEnd);
            }

            if (categoryId.HasValue)
                query = query.Where(mf => mf.Message.CategoryId == categoryId.Value);

    
            if (unreadOnly == true)
                query = query.Where(mf => !mf.Message.IsRead);

            if (starredOnly == true)
                query = query.Where(mf => mf.IsStarred);

            query = sort == "asc"
                ? query.OrderBy(mf => mf.Message.SendDate)
                : query.OrderByDescending(mf => mf.Message.SendDate);

            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            page = Math.Max(1, Math.Min(page, totalPages));

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(mf => new MessageListDto
                {
                    MessageId = mf.Message.Id,
                    MessageFolderId = mf.Id,
                    ReceiverName = mf.Message.Receiver.FirstName + " " + mf.Message.Receiver.LastName,
                    Subject = mf.Message.Subject,
                    BodyPreview = mf.Message.Body,
                    SendDate = mf.Message.SendDate,
                    IsRead = mf.Message.IsRead,   //alıcı okundu durumu
                    IsStarred = mf.IsStarred,
                    CategoryName = mf.Message.Category != null ? mf.Message.Category.Name : null
                })
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            ViewBag.FilterReceiver = receiver;
            ViewBag.FilterSubject = subject;
            ViewBag.FilterDateFrom = dateFrom?.ToString("yyyy-MM-dd");
            ViewBag.FilterDateTo = dateTo?.ToString("yyyy-MM-dd");
            ViewBag.FilterCategoryId = categoryId;
            ViewBag.FilterUnreadOnly = unreadOnly == true;
            ViewBag.FilterStarredOnly = starredOnly == true;
            ViewBag.FilterSort = sort;

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
        [HttpGet]
        public async Task<IActionResult> Starred()
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var items = await _context.MessageFolders
                .Where(mf => mf.UserId == userId
                             && mf.IsStarred                
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

        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> ToggleStar(int messageFolderId)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);
            var folder = await _context.MessageFolders
                .FirstOrDefaultAsync(mf => mf.Id == messageFolderId && mf.UserId == userId);

            if (folder == null)
                return NotFound();

            folder.IsStarred = !folder.IsStarred;
            await _context.SaveChangesAsync();

            return Json(new { success = true, isStarred = folder.IsStarred });
        }

        public async Task<IActionResult> Trash()
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var items = await _context.MessageFolders
                .Where(mf => mf.UserId == userId
                             && mf.IsDeleted)   
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToTrash(int messageFolderId)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var folder = await _context.MessageFolders
                .FirstOrDefaultAsync(mf => mf.Id == messageFolderId && mf.UserId == userId);

            if (folder == null)
                return NotFound();

            folder.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreFromTrash(int messageFolderId)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var folder = await _context.MessageFolders
                .FirstOrDefaultAsync(mf => mf.Id == messageFolderId && mf.UserId == userId);

            if (folder == null)
                return NotFound();

            folder.IsDeleted = false;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string query)
        {
            var currentUserId = int.Parse(_userManager.GetUserId(User)!);

            if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
                return Json(Array.Empty<object>());

            query = query.Trim();

            var users = await _context.Users
                .Where(u => u.Id != currentUserId
                            && (EF.Functions.Like(u.Email!, $"%{query}%")
                                || EF.Functions.Like(u.FirstName, $"%{query}%")
                                || EF.Functions.Like(u.LastName, $"%{query}%")))
                .OrderBy(u => u.FirstName)
                .Take(8)
                .Select(u => new
                {
                    id = u.Id,
                    name = u.FirstName + " " + u.LastName,
                    email = u.Email,
                    avatarUrl = string.IsNullOrEmpty(u.ProfileImageUrl) ? "/avatar.png" : u.ProfileImageUrl
                })
                .ToListAsync();

            return Json(users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportMessage(int messageId, string reason)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var message = await _context.UserMessages
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (message == null)
                return NotFound();
            //alıcı şikayet edebiliyor
            if ( message.ReceiverId != userId)
                return Forbid();

            if (string.IsNullOrWhiteSpace(reason))
                return Json(new { success = false, error = "Şikayet nedeni zorunlu." });

            var alreadyReported = await _context.Reports
                .AnyAsync(r => r.MessageId == messageId
                               && r.ReportedByUserId == userId
                               && r.Status == ReportStatus.Pending);

            if (alreadyReported)
                return Json(new { success = false, error = "Bu mesajı zaten şikayet ettiniz." });

            _context.Reports.Add(new Report
            {
                MessageId = messageId,
                ReportedByUserId = userId,
                Reason = reason,
                ReportDate = DateTime.UtcNow,
                Status = ReportStatus.Pending
            });

           
            message.IsReported = true;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
