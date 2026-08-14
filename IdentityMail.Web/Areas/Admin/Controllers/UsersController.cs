using IdentityMail.Web.Context;
using IdentityMail.Web.DTOs.AdminDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    public class UsersController(AppDbContext _context, UserManager<AppUser> _userManager) : BaseAdminController
    {
        [HttpGet]
        public async Task<IActionResult> Index(string? word, string? role)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(word))
            {
                query = query.Where(u => u.FirstName.Contains(word)
                                          || u.LastName.Contains(word)
                                          || u.Email!.Contains(word));
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                query = query.Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id
                               && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == role)));
            }

            var users = await query
                .OrderBy(u => u.FirstName)
                .Select(u => new UserListDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName!,
                    Email = u.Email!,
                    ImageUrl = u.ProfileImageUrl,
                    IsActive = u.IsActive,
                    Role = _context.UserRoles
                        .Where(ur => ur.UserId == u.Id)
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .FirstOrDefault() ?? "Rolsüz"
                })
                .ToListAsync();

            ViewBag.UserCount = await _context.Users.CountAsync();
            ViewBag.FilterWord = word;
            ViewBag.FilterRole = role;

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            var currentUserId = int.Parse(_userManager.GetUserId(User)!);
            if (id == currentUserId)
            {
                TempData["ErrorMessage"] = "Kendi hesabınızı pasif yapamazsınız.";
                return RedirectToAction("Index");
            }

            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }
                

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kullanıcı durumu güncellendi.";
            return RedirectToAction("Index");
        }
       
    }
}