using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    public class DashboardController(AppDbContext _context) : BaseAdminController
    {
        public IActionResult Index()
        {
            ViewBag.Users = _context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "User"))).Count(); //toplam kullanıcı
            ViewBag.ActiveUsers = _context.Users.Where(u => u.IsActive
                && _context.UserRoles.Any(ur => ur.UserId == u.Id
                    && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "User"))).Count(); //aktif kullanıcı

            ViewBag.TotalMessages = _context.UserMessages.Count(); //toplam mesaj
            ViewBag.TodayMessages = _context.UserMessages.Where(m => m.SendDate.Date == DateTime.UtcNow.Date).Count(); //bugünkü mesaj
            ViewBag.UnreadMessages = _context.UserMessages.Where(m => m.IsRead == false).Count(); //okunmamış mesaj
            ViewBag.TrashMessages = _context.MessageFolders.Where(m => m.IsDeleted == true).Count(); //çöp kutusundaki mesajlar
            //en çok mesaj gönderen kullanıcılar
            var topSenders = _context.UserMessages
    .GroupBy(m => m.SenderId)
    .Select(g => new { SenderId = g.Key, Count = g.Count() })
    .OrderByDescending(x => x.Count)
    .Join(_context.Users,
        x => x.SenderId,
        u => u.Id,
        (x, u) => new TopSender
        {
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email!,
            ImageUrl = u.ProfileImageUrl,
            MessageCount = x.Count
        })
    .ToList();
            //en çok kullanılan kategoriler
            var topCategories = _context.UserMessages
    .Where(m => m.CategoryId != null)   // kategorisiz mesajlar dahil edilmesin
    .GroupBy(m => m.CategoryId)
    .Select(g => new { CategoryId = g.Key, Count = g.Count() })
    .OrderByDescending(x => x.Count)
    .Take(5)
    .Join(_context.Categories,
        x => x.CategoryId,
        c => c.Id,
        (x, c) => new TopCategory
        {
            Name = c.Name,
            ColorHex = c.ColorHex,
            MessageCount = x.Count
        })
    .ToList();
            var model = new UserDto
            {
                TopSenders = topSenders,
                TopCategories = topCategories
            };
            return View(model);
        }
    }
}
