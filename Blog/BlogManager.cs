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

        public ActionResult<ViewBlogModel> GetViewModel(int id, ClaimsPrincipal principals)
        {
            var blog = BlogService.GetBlog(id);
            if (blog == null)
                return new NotFoundResult();
            return new ViewBlogModel() { Blog = blog, BlogId=blog.Id, Comment = "" };
        }

        public async Task<List<BlogModel>> GetUserBlogs(ClaimsPrincipal principals)
        {
            ApplicationUser u = await UserManager.GetUserAsync(principals);
            if (u == null)
                return new List<BlogModel>();
            return BlogService.UserBlogs(u);
        }

        public async Task<List<CommentModel>> GetUserComments(ClaimsPrincipal principals)
        {
            ApplicationUser u = await UserManager.GetUserAsync(principals);
            if (u == null)
                return new List<CommentModel>();
            return BlogService.UserComments(u);
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

        public async void DeleteUserBlogsAndComments(ClaimsPrincipal principals)
        {

            foreach (var blog in await GetUserBlogs(principals))
            {
                BlogService.DeleteBlog(blog);
            }
            foreach (var comment in await GetUserComments(principals))
            {
                BlogService.DeleteComments(await GetUserComments(principals));
            }
        }

        public async Task<BlogModel> CreateBlog(CreateBlogModel model, ClaimsPrincipal principals)
        {
            ApplicationUser a = await UserManager.GetUserAsync(principals);
            BlogModel m = new BlogModel()
            {
                Author = await UserManager.GetUserAsync(principals),
                LastChange = DateTime.Now,
                Content = model.Content,
                Title = model.Title
            };
            m = await BlogService.CreateBlog(m);
            return m;
        }

        public async Task<ActionResult<BlogModel>> DeleteBlog(int id, ClaimsPrincipal principals)
        {
            ApplicationUser a = await UserManager.GetUserAsync(principals);
            var oldBlog = BlogService.GetBlog(id);
            if (oldBlog == null)
                return new NotFoundResult();
            var authorization = await AuthorizatoinService.AuthorizeAsync(principals, oldBlog, Operations.Delete);
            if (!authorization.Succeeded)
                return new ForbidResult();
            BlogService.DeleteBlog(oldBlog);
            return null;
        }

        public async Task<ActionResult<BlogModel>> AddComment(ViewBlogModel model, ClaimsPrincipal claimsPrincipal)
        {
            var oldBlog = BlogService.GetBlog(model.BlogId);
            if (oldBlog == null)
                return new NotFoundResult();
            ApplicationUser a = await UserManager.GetUserAsync(claimsPrincipal);
            if (a == null)
                return new ChallengeResult();
            CommentModel m = new CommentModel()
            {
                Author = a,
                Content = model.Comment,
                Created = DateTime.Now
            };
            oldBlog.Comments.Add(m);
            await BlogService.AddComment(oldBlog, m);
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
