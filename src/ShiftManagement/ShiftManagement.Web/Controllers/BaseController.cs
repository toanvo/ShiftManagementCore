namespace ShiftManagement.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class BaseController : Controller
    {
        public ClaimsPrincipal GetCurrentUser
        {
            get { return HttpContext.User; }
        }
    }
}
