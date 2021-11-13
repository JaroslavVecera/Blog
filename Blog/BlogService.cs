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

        public async Task<BlogModel> CreateBlog(BlogModel model)
        {
            DBContext.Add(model);
            await DBContext.SaveChangesAsync();
            return model;
        }
    }
}
