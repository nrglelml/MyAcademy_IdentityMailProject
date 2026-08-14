using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityMail.Web.ViewComponents.AdminLayoutComponent
{
    public class _AdminNavbar(UserManager<AppUser> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var user = await _userManager.GetUserAsync((ClaimsPrincipal)User);
            if (user == null)
            {
                return Content(string.Empty);
            }


            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.image = user.ProfileImageUrl;
            ViewBag.Email = user.Email;
            return View();
        }
    }
}
