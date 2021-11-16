using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        ProfileManager ProfileManager { get; set; }

        public ProfileController(ProfileManager profileManager)
        {
            ProfileManager = profileManager;
        }

        [AllowAnonymous]
        public IActionResult Index(string id)
        {
            if (id == null)
                return new BadRequestResult();
            var actionResult = ProfileManager.GetViewModel(id, User);
            if (actionResult.Result == null)
                return View(actionResult.Value);
            return actionResult.Result;
        }
    }
}
