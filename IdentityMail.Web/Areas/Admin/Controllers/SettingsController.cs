using IdentityMail.Web.Areas.User.Controllers;
using IdentityMail.Web.DTOs.AdminDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    public class SettingsController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : BaseAdminController
    {

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new UpdateProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ExistingProfileImageUrl = user.ProfileImageUrl
            };

            ViewBag.Email = user.Email;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UpdateProfileDto updateProfile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            user.FirstName = updateProfile.FirstName;
            user.LastName = updateProfile.LastName;
            if (updateProfile.ProfileImage != null && updateProfile.ProfileImage.Length > 0)
            {
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfileImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                user.ProfileImageUrl = await SaveImageFileAsync(updateProfile.ProfileImage);
            }
            else
            {
                user.ProfileImageUrl = updateProfile.ExistingProfileImageUrl;
            }

            ViewBag.email = user.Email;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Bilgileriniz başarıyla güncellendi.";
                return RedirectToAction("Profile");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(updateProfile);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UpdateProfileDto updateProfile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            if (updateProfile.CurrentPassword == updateProfile.NewPassword)
            {
                ModelState.AddModelError(nameof(updateProfile.NewPassword), "Yeni şifreniz mevcut şifrenizle aynı olamaz.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Email = user.Email;
                updateProfile.FirstName = user.FirstName;
                updateProfile.LastName = user.LastName;
                updateProfile.ExistingProfileImageUrl = user.ProfileImageUrl;

                TempData["ActiveTab"] = "password";

                return View("Profile", updateProfile);
            }
            var result = await _userManager.ChangePasswordAsync(user, updateProfile.CurrentPassword, updateProfile.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("Profile");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code.StartsWith("Password"))
                        ModelState.AddModelError(nameof(updateProfile.NewPassword), error.Description);
                    else
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Profile", updateProfile);
            }
        }
      
        public async Task<IActionResult> SafeDelete(UpdateProfileDto updateProfile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                TempData["SuccessMessage"] = "Hesabınız başarıyla silindi.";
                return Redirect("/Auth/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Profile", updateProfile);
            }
        }
        private async Task<string> SaveImageFileAsync(IFormFile imageFile)
        {
            var resource = Directory.GetCurrentDirectory();
            var extension = Path.GetExtension(imageFile.FileName);
            var imageName = Guid.NewGuid() + extension;
            var folder = Path.Combine(resource, "wwwroot", "adminProfileImages");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var saveLocation = Path.Combine(folder, imageName);

            using (var stream = new FileStream(saveLocation, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/adminProfileImages/" + imageName;
        }
    }
}