using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMail.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public abstract class BaseUserController : Controller
    {
      
    }
}
