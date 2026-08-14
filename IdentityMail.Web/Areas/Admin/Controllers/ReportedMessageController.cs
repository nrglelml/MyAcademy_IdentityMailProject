using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    public class ReportedMessageController(AppDbContext _context) : BaseAdminController
    {
        public async Task<IActionResult> Index()
        {
            var messages = await _context.UserMessages.Where(x => x.IsReported == true)
        .OrderByDescending(x => x.SendDate)
        .Join(_context.Reports, y => y.Id, r => r.MessageId, (y, r) => new ReportedMessageListDto
        {
            MessageId = y.Id,
            Sender = y.Sender.Email,
            ReportByUser = r.ReportedByUser.Email,
            Reason = r.Reason,
            Subject = y.Subject,
            ReportDate = r.ReportDate
        }).ToListAsync();

            return View(messages);

        }
        public async Task<IActionResult> Details(int id)
        {
            var message = await _context.UserMessages
                .Where(x => x.Id == id && x.IsReported == true)
                .Join(_context.Reports, y => y.Id, r => r.MessageId, (y, r) => new ReportMessageDetail
                {
                    MessageId = y.Id,
                    Sender = y.Sender.Email,
                    ReportByUser = r.ReportedByUser.Email,
                    Reason = r.Reason,
                    Subject = y.Subject,
                    ReportDate = r.ReportDate,
                    Body = y.Body
                }).FirstOrDefaultAsync();
            if (message == null)
            {
                TempData["ErrorMessage"] = "Mesaj bulunamadı.";
                return RedirectToAction("Index");
            }
            return View(message);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.UserMessages
                .Where(x => x.Id == id && x.IsReported == true)
                .FirstOrDefaultAsync();
            if (message == null)
            {
                TempData["ErrorMessage"] = "Mesaj bulunamadı.";
                return RedirectToAction("Index");
            }
            _context.UserMessages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
