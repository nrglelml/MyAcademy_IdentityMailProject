using IdentityMail.Web.Context;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.ViewComponents.LayoutComponent
{
    public class _Sidebar(UserManager<AppUser> _userManager, AppDbContext _context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdString = _userManager.GetUserId(UserClaimsPrincipal);
            var userId = int.Parse(userIdString!);

            var inboxCount = await _context.MessageFolders
                .Where(x => x.UserId == userId
                            && x.FolderType == FolderType.Inbox
                            && !x.IsDeleted
                            && !x.Message.IsRead)
                .CountAsync();

            var sentCount = await _context.MessageFolders
                .Where(x => x.UserId == userId
                            && x.FolderType == FolderType.Sent
                            && !x.IsDeleted)
                .CountAsync();

            var draftCount = await _context.Drafts
                .Where(x => x.SenderId == userId)
                .CountAsync();

            var starredCount = await _context.MessageFolders
                .Where(x => x.UserId == userId
                            && x.IsStarred
                            && !x.IsDeleted)
                .CountAsync();

            var trashCount = await _context.MessageFolders
                .Where(x => x.UserId == userId && x.IsDeleted)
                .CountAsync();

            ViewBag.InboxCount = inboxCount;
            ViewBag.SentCount = sentCount;
            ViewBag.DraftCount = draftCount;
            ViewBag.StarredCount = starredCount;
            ViewBag.TrashCount = trashCount;

            return View();
        }
    }
}
