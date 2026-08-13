using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.DraftDtos;
using IdentityMail.Web.Entities;
using IdentityMail.Web.Services.MessageServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.User.Controllers
{
    public class DraftController(UserManager<AppUser> _userManager, AppDbContext _context, IMessageService _messageService) : BaseUserController
    {

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var draftEntities = await _context.Drafts
                .Where(d => d.SenderId == userId)
                .OrderByDescending(d => d.LastEditedDate)
                .ToListAsync();

            var emails = draftEntities
                .Where(d => !string.IsNullOrWhiteSpace(d.ReceiverEmail))
                .Select(d => d.ReceiverEmail!)
                .Distinct()
                .ToList();

            var matchingUsers = await _context.Users
                .Where(u => emails.Contains(u.Email!))
                .ToDictionaryAsync(u => u.Email!, u => $"{u.FirstName} {u.LastName}");

            var items = draftEntities.Select(d => new DraftListDto
            {
                DraftId = d.Id,
                RecipientDisplay = string.IsNullOrWhiteSpace(d.ReceiverEmail)
                    ? null
                    : (matchingUsers.TryGetValue(d.ReceiverEmail!, out var name) ? name : d.ReceiverEmail),
                Subject = d.Subject,
                BodyPreview = d.Body,
                LastEditedDate = d.LastEditedDate
            }).ToList();

            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDraft(int id)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var draft = await _context.Drafts
                .FirstOrDefaultAsync(d => d.Id == id && d.SenderId == userId);

            if (draft == null)
            {
                TempData["ErrorMessage"] = "Taslak bulunamadı.";
                return RedirectToAction("Drafts");
            }

            _context.Drafts.Remove(draft);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Taslak silindi.";
            return RedirectToAction("Drafts");
        }
    }
}
