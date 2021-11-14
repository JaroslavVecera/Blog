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
    }
}
