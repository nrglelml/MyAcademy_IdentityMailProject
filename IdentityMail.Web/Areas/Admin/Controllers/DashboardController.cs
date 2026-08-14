using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Areas.Admin.Controllers
{
    public class DashboardController :BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
