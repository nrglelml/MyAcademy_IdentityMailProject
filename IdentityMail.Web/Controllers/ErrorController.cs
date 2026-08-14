using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404(int code)
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
