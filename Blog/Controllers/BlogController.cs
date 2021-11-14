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
    public class BlogController : Controller
    {
        BlogManager BlogManager { get; set; }

        public BlogController(BlogManager blogManager)
        {
            BlogManager = blogManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new CreateBlogModel());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new BadRequestResult();
            var actionResult = await BlogManager.GetEditModel(id.Value, User);
            if (actionResult.Result == null)
                return View(actionResult.Value);
            return actionResult.Result;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogModel cbm)
        {
            await BlogManager.CreateBlog(cbm, User);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogModel ebm)
        {
            var actionResult = await BlogManager.EditBlog(ebm, User);
            if (actionResult.Result == null)
                return RedirectToAction("Edit", new { ebm.Id });
            return actionResult.Result;
        }
    }
}
