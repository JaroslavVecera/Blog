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
        public IActionResult Index(int? id)
        {
            if (id == null)
                return new BadRequestResult();
            var actionResult = BlogManager.GetViewModel(id.Value, User);
            if (actionResult.Result == null)
                return View(actionResult.Value);
            return actionResult.Result;
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
        public async Task<IActionResult> Comment(ViewBlogModel vbm)
        {
            await BlogManager.AddComment(vbm, User);
            return RedirectToAction("Index", new { Id = vbm.BlogId });
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogModel cbm)
        {
            var model = await BlogManager.CreateBlog(cbm, User);
            return RedirectToAction("Index", new { model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, ViewBlogModel vbm)
        {
            if (id == null)
                return NotFound();
            var actionResult = await BlogManager.DeleteBlog(id.Value, User);
            if (actionResult == null || actionResult.Result == null)
                return RedirectToAction("Index", "Home");
            return actionResult.Result;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogModel ebm)
        {
            var actionResult = await BlogManager.EditBlog(ebm, User);
            if (actionResult.Result == null)
                return RedirectToAction("Index", new { ebm.Id });
            return actionResult.Result;
        }
    }
}
