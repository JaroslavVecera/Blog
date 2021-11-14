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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blog
{
    public class BlogManager
    {
        public UserManager<ApplicationUser> UserManager { get; set; }
        BlogService BlogService { get; set; }
        IAuthorizationService AuthorizatoinService { get; set; }

        public BlogManager(UserManager<ApplicationUser> User, BlogService blogService, IAuthorizationService authorizationService)
        {
            UserManager = User;
            BlogService = blogService;
            AuthorizatoinService = authorizationService;
        }

        public List<BlogModel> GetAllBlogs()
        {
            return BlogService.AllBlogs();
        }

        public List<BlogModel> GetUserBlogs(ApplicationUser u)
        {
            return BlogService.AllBlogs().Where(blog => blog.Author == u).ToList();
        }

        public async Task<ActionResult<EditBlogModel>> GetEditModel(int id, ClaimsPrincipal principals)
        {
            var blog = BlogService.GetBlog(id);
            if (blog == null)
                return new NotFoundResult();
            var authorization = await AuthorizatoinService.AuthorizeAsync(principals, blog, Operations.Modify);
            if (!authorization.Succeeded)
            {
                if (principals.Identity.IsAuthenticated)
                    return new ForbidResult();
                else
                    return new ChallengeResult();

            }
            return new EditBlogModel() { Id = blog.Id, Title = blog.Title, Content = blog.Content };
        }

        public async Task<BlogModel> CreateBlog(CreateBlogModel model, ClaimsPrincipal claimsPrincipal)
        {
            ApplicationUser a = await UserManager.GetUserAsync(claimsPrincipal);
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

        public async Task<ActionResult<BlogModel>> AddComment(ViewBlogModel model, ClaimsPrincipal claimsPrincipal)
        {
            var oldBlog = BlogService.GetBlog(model.Blog.Id);
            if (oldBlog == null)
                return new NotFoundResult();
            ApplicationUser a = await UserManager.GetUserAsync(claimsPrincipal);
            if (a == null)
                return new ChallengeResult();
            CommentModel m = new CommentModel()
            {
                Author = a,
                Content = model.Comment,
            };
            oldBlog.Comments.Add(m);
            await BlogService.EditBlog(oldBlog);
            return oldBlog;
        }

        public async Task<ActionResult<EditBlogModel>> EditBlog(EditBlogModel model, ClaimsPrincipal principals)
        {
            var oldBlog = BlogService.GetBlog(model.Id);
            if (oldBlog == null)
                return new NotFoundResult();
            var authorization = await AuthorizatoinService.AuthorizeAsync(principals, oldBlog, Operations.Modify);
            if (!authorization.Succeeded)
                return new ForbidResult();
            oldBlog.Title = model.Title;
            oldBlog.Content = model.Content;
            await BlogService.EditBlog(oldBlog);
            return model;
        }
    }
}
