using Blog.Data;
using Blog.Models;
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

namespace Blog
{
    public class BlogService
    {
        ApplicationDbContext DBContext { get; set; }

        public BlogService(ApplicationDbContext dbContext)
        {
            DBContext = dbContext;
        }

        public BlogModel GetBlog(int id)
        {
            return DBContext.Blogs
                .Include(blog => blog.Author)
                .Include(blog => blog.Comments).ThenInclude(comment => comment.Author)
                .FirstOrDefault(blog => blog.Id == id);
        }


        public List<BlogModel> AllBlogs()
        {
            return DBContext.Blogs
                .Include(blog => blog.Author)
                .Include(blog => blog.Comments).ThenInclude(comment => comment.Author)
                .OrderByDescending(blog => blog.LastChange)
                .ToList();
        }

        public List<BlogModel>UserBlogs(ApplicationUser u)
        {
            return DBContext.Blogs
                .Include(blog => blog.Author)
                .Include(blog => blog.Comments).ThenInclude(comment => comment.Author)
                .Where(blog => blog.Author == u)
                .OrderByDescending(blog => blog.LastChange)
                .ToList();
        }

        public List<CommentModel> UserComments(ApplicationUser u)
        {
            return DBContext.Comments
                .Include(comment => comment.Author)
                .Where(comment => comment.Author == u)
                .OrderByDescending(comment => comment.Created)
                .ToList();
        }

        public async Task<BlogModel> CreateBlog(BlogModel model)
        {
            DBContext.Add(model);
            await DBContext.SaveChangesAsync();
            return model;
        }

        public async Task<BlogModel> EditBlog(BlogModel model)
        {
            DBContext.Update(model);
            await DBContext.SaveChangesAsync();
            return model;
        }

        public void DeleteBlog(BlogModel model)
        {
            DBContext.RemoveRange(model.Comments);
            DBContext.Remove(model);
            DBContext.SaveChanges();
        }

        public void DeleteComments(List<CommentModel> comments)
        {
            DBContext.RemoveRange(comments);
            DBContext.SaveChanges();
        }

        public async Task<BlogModel> AddComment(BlogModel blog, CommentModel comment)
        {
            DBContext.Update(blog);
            DBContext.Add(comment);
            await DBContext.SaveChangesAsync();
            return blog;
        }
    }
}
