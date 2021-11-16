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
    public class ProfileService
    {
        ApplicationDbContext DBContext { get; set; }

        public ProfileService(ApplicationDbContext dbContext)
        {
            DBContext = dbContext;
        }

        public ApplicationUser GetProfile(string id)
        {
            return DBContext.Users
                .FirstOrDefault(user => user.Id == id);
        }

        public async Task<ApplicationUser> EditProfile(ApplicationUser model)
        {
            return null;
        }

        public void DeleteProfile(ApplicationUser model)
        {

        }
    }
}
