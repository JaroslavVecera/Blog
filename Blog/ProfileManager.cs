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
    public class ProfileManager
    {
        public UserManager<ApplicationUser> UserManager { get; set; }
        ProfileService ProfileService { get; set; }
        IAuthorizationService AuthorizatoinService { get; set; }
        BlogManager BlogManager { get; set; }

        public ProfileManager(UserManager<ApplicationUser> User, BlogManager blogManager, ProfileService profileService, IAuthorizationService authorizationService)
        {
            UserManager = User;
            ProfileService = profileService;
            AuthorizatoinService = authorizationService;
            BlogManager = blogManager;
        }

        public ActionResult<ProfileModel> GetViewModel(string id, ClaimsPrincipal principals, bool allBlogsAndComments)
        {
            var user = ProfileService.GetProfile(id);
            if (user == null)
                return new NotFoundResult();
            List<BlogModel> latestBlogs = BlogManager.GetUserBlogs(user);
            if (allBlogsAndComments)
                latestBlogs = latestBlogs.Take(5).ToList();
            var allBlogs = BlogManager.GetAllBlogs();
            var comments = BlogManager.GetUserComments(user);
            if (allBlogsAndComments)
                comments = comments.Take(5).ToList();
            var latestLinkedComments = comments.Select(comment => new CommentLinkModel() { BlogId = allBlogs.First(blog => blog.Comments.Contains(comment)).Id, Comment = comment }).ToList();

            return new ProfileModel()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AboutMe = user.AboutMe,
                LiveIn = user.LiveIn,
                Birth = user.Birth,
                LatestBlogs = latestBlogs,
                LatestComments = latestLinkedComments
            };
        }
    }
}
