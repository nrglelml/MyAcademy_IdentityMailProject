using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.UserMessagesDtos;
using IdentityMail.Web.Entities;
using IdentityMail.Web.Services.MessageServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.User.Controllers
{
    public class MessageDetailController(UserManager<AppUser> _userManager, AppDbContext _context, IMessageService _messageService) : BaseUserController
    {
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var message = await _context.UserMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
                return NotFound();

     
            if (message.SenderId != userId && message.ReceiverId != userId)
                return Forbid();


            if (message.ReceiverId == userId && !message.IsRead)
            {
                message.IsRead = true;
                message.ReadDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            var thread = await BuildThreadAsync(message);

            var vm = new MessageDetailDto
            {
                MessageId = message.Id,
                Subject = message.Subject,
                CategoryName = message.Category?.Name,
                IsViewerSender = message.SenderId == userId,
                OtherPartyEmail = message.SenderId == userId ? message.Receiver!.Email! : message.Sender!.Email!,
                IsRead = message.IsRead,
                ReadDate = message.ReadDate,
                Thread = thread
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, string body)
        {
            var userId = int.Parse(_userManager.GetUserId(User)!);

            var original = await _context.UserMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (original == null)
                return NotFound();

            if (original.SenderId != userId && original.ReceiverId != userId)
                return Forbid();

            if (string.IsNullOrWhiteSpace(body))
            {
                TempData["ErrorMessage"] = "Yanıt boş olamaz.";
                return RedirectToAction("Detail", new { id });
            }

            // Yanıt "karşı taraf"a gider: sen alıcıysan gönderene, sen
            // gönderense alıcıya.
            var otherPartyEmail = original.SenderId == userId
                ? original.Receiver!.Email
                : original.Sender!.Email;

            var dto = new ComposeMessageDto
            {
                ReceiverEmail = otherPartyEmail,
                Subject = original.Subject.StartsWith("Re:") ? original.Subject : $"Re: {original.Subject}",
                Body = body,
                ParentMessageId = original.Id,
                CategoryId = original.CategoryId
            };

            var result = await _messageService.SendMessageAsync(userId, dto);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction("Detail", new { id });
            }

            TempData["SuccessMessage"] = "Yanıt gönderildi.";
            return RedirectToAction("Detail", new { id = result.MessageId });
        }

        // Yukarı doğru (ebeveyn zinciri) konuşma geçmişini toplar.
        // En eskisi listenin başında, en yenisi (görüntülenen mesaj) sonunda olur.
        private async Task<List<MessageThreadItem>> BuildThreadAsync(UserMessage message)
        {
            var chain = new List<UserMessage> { message };
            var current = message;

            while (current.ParentMessageId.HasValue)
            {
                var parent = await _context.UserMessages
                    .Include(m => m.Sender)
                    .FirstOrDefaultAsync(m => m.Id == current.ParentMessageId.Value);

                if (parent == null) break;

                chain.Insert(0, parent);
                current = parent;
            }

            return chain.Select(m => new MessageThreadItem
            {
                SenderName = m.Sender != null ? $"{m.Sender.FirstName} {m.Sender.LastName}" : "Bilinmeyen",
                Body = m.Body,
                SendDate = m.SendDate,
                IsCurrentMessage = m.Id == message.Id
            }).ToList();
        }
    }
}
