using IdentityMail.Web.DTOs.UserDtos;
using IdentityMail.Web.Entities;
using IdentityMail.Web.Services.EmailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager,IEmailSender _emailSender) : Controller
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
                   
                    if (error.Code == "DuplicateEmail")
                    {
                        ModelState.AddModelError(nameof(registerDto.Email), "Bu e-posta adresi zaten kullanılmaktadır.");
                    }
                    
                    else if (error.Code == "DuplicateUserName")
                    {
                        ModelState.AddModelError(nameof(registerDto.UserName), "Bu kullanıcı adı zaten alınmış.");
                    }
                   
                    else if (error.Code.StartsWith("Password"))
                    {
                        ModelState.AddModelError(nameof(registerDto.Password), error.Description);
                    }
                   
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(registerDto);
            }
            else
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Auth",
                    new { userId = user.Id, token = token },
                    Request.Scheme);

                
                string emailBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2>Aramıza Hoş Geldiniz, {user.FirstName}!</h2>
                    <p>Hesabınızı aktif etmek ve giriş yapabilmek için lütfen aşağıdaki butona tıklayın:</p>
                    <a href='{confirmationLink}' style='background-color: #4f46e5; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; margin-top: 10px;'>Hesabımı Doğrula</a>
                </div>";

                await _emailSender.SendEmailAsync(user.Email, "NMail - Hesap Doğrulama", emailBody);

                TempData["SuccessMessage"] = "Kayıt başarılı! Lütfen e-postanızı kontrol edin.";
                return RedirectToAction("Login", "Auth");
            }
           
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Geçersiz doğrulama bağlantısı.";
                return RedirectToAction("Login", "Auth");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "E-posta adresiniz başarıyla doğrulandı. Giriş yapabilirsiniz.";
            }
            else
            {
                TempData["ErrorMessage"] = "E-posta doğrulama linki geçersiz veya süresi dolmuş.";
            }

            return RedirectToAction("Login", "Auth");
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Lütfen e-posta adresinizi girin.";
                return RedirectToAction("Login", "Auth");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["SuccessMessage"] = "Eğer bu e-posta adresi sistemde kayıtlıysa doğrulama linki tekrar gönderilmiştir.";
                return RedirectToAction("Login", "Auth");
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["ErrorMessage"] = "Bu e-posta adresi zaten doğrulanmış. Giriş yapabilirsiniz.";
                return RedirectToAction("Login", "Auth");
            }

            // Yeni Token üret ve Link Hazırla
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Auth",
                new { userId = user.Id, token = token },
                Request.Scheme);

            string emailBody = $@"
    <div style='font-family: Arial, sans-serif; padding: 20px;'>
        <h2>E-posta Doğrulama Talebi</h2>
        <p>Hesabınızı aktif etmek için lütfen aşağıdaki butona tıklayın:</p>
        <a href='{confirmationLink}' style='background-color: #4f46e5; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; margin-top: 10px;'>Hesabımı Doğrula</a>
    </div>";

            await _emailSender.SendEmailAsync(user.Email, "NMail - Hesap Doğrulama (Tekrar)", emailBody);

            TempData["SuccessMessage"] = "Yeni doğrulama bağlantısı e-posta adresinize gönderildi.";
            return RedirectToAction("Login", "Auth");
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
            if (result.Succeeded)
            {
                return RedirectToAction("Inbox", "Message", new { area = "User" });
            }
            // E-posta Onayı Kontrolü
            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Giriş yapabilmek için önce e-posta adresinizi doğrulamanız gerekmektedir.");
                return View(loginDto);
            }
            ModelState.AddModelError(string.Empty, "Email adresi veya şifre hatalı!");
            return View(loginDto);

        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Lütfen geçerli bir e-posta adresi girin.");
                return View();
            }
            var user=await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email sistemde kayıtlı değil!");
                return View();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Auth", new { token = token, email = user.Email }, Request.Scheme);

            string emailBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Şifre Sıfırlama Talebi</h2>
                <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayabilirsiniz:</p>
                <a href='{resetLink}' style='background-color: #4f46e5; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; margin-top: 10px;'>Şifremi Sıfırla</a>
            </div>";

            await _emailSender.SendEmailAsync(user.Email, "NMail - Şifre Sıfırlama", emailBody);

            ViewBag.Message = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.";
            return View();

        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Geçersiz şifre sıfırlama bağlantısı.";
                return RedirectToAction("Login", "Auth");
            }

            var model = new ResetPasswordDto
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordDto);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Login", "Auth");
            }

            // Identity üzerinden şifre sıfırlama işlemi
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla güncellendi! Yeni şifrenizle giriş yapabilirsiniz.";
                return RedirectToAction("Login", "Auth");
            }

            foreach (var error in result.Errors)
            {
                if (error.Code.StartsWith("Password"))
                    ModelState.AddModelError(nameof(resetPasswordDto.NewPassword), error.Description);
                else
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(resetPasswordDto);
        }

    }
}
