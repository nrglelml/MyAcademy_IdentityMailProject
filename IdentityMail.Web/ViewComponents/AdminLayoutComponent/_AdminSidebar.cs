using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.ViewComponents.AdminLayoutComponent
{
    public class _AdminSidebar:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
