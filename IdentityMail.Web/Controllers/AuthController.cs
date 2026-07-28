using IdentityMail.Web.DTOs.UserDtos;
using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    public class AuthController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager) : Controller
    {
  
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)    
        {
            if(registerDto.Password != registerDto.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler eşleşmiyor.");
                return View(registerDto);
            }
            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return View(registerDto);
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
           
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
           var user=await _userManager.FindByEmailAsync(loginDto.Email);
            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email sistemde kayıtlı değil!");
                return View(loginDto);
            }
            var result =await _signInManager.PasswordSignInAsync(user, loginDto.Password,false,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email adresi veya şifre hatalı!");
                return View(loginDto);
            }
            else
            {
                return RedirectToAction("Index", "Message");
            }
               
        }
    }
}
