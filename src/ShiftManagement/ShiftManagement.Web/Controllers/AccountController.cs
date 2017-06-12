namespace ShiftManagement.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }        
    }
}
