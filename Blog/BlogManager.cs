using Blog.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using System.Security.Claims;

namespace Blog
{
    public class BlogManager
    {
        public UserManager<ApplicationUser> UserManager { get; set; }
        BlogService BlogService { get; set; }

        public BlogManager(UserManager<ApplicationUser> User, BlogService blogService)
        {
            UserManager = User;
            BlogService = blogService;
        }

        public async Task<BlogModel> CreateBlog(CreateBlogModel model, ClaimsPrincipal claimsPrincipal)
        {
            BlogModel m = new BlogModel()
            {
                Author = await UserManager.GetUserAsync(claimsPrincipal),
                LastChange = DateTime.Now,
                Content = model.Content,
                Title = model.Title
            };
            m = await BlogService.CreateBlog(m);
            return m;
        }
    }
}
