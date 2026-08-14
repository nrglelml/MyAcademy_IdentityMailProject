using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.AdminDtos
{
    public class UpdateProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ExistingProfileImageUrl { get; set; }
        public IFormFile? ProfileImage { get; set; }
        [Required(ErrorMessage = "Mevcut şifrenizi girmelisiniz.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre alanı zorunludur.")]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmNewPassword { get; set; }
    }
}
