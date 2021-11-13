using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        BlogManager BlogManager { get; set; }

        public BlogController(BlogManager blogManager)
        {
            BlogManager = blogManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new CreateBlogModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogModel cbm)
        {
            await BlogManager.CreateBlog(cbm, User);
            return View();
        }
    }
}
