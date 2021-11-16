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
    public class HomeController : Controller
    {
        BlogManager BlogManager { get; set; }

        public IActionResult Index()
        {
            return View(new BlogsModel() { Blogs = BlogManager.GetAllBlogs() });
        }

        [Authorize]
        public async Task<IActionResult> MyBlogs()
        {
            return View(new BlogsModel() { Blogs = await BlogManager.GetUserBlogs(User) });
        }

        public HomeController(BlogManager blogManager)
        {
            BlogManager = blogManager;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
