using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public static class Operations
    {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement() { Name = "Create" };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement() { Name = "Delete" };
        public static OperationAuthorizationRequirement Modify = new OperationAuthorizationRequirement() { Name = "Modify" };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement() { Name = "Read" };
    }

    public class BlogAuthorization : AuthorizationHandler<OperationAuthorizationRequirement, BlogModel>
    {
        UserManager<ApplicationUser> UserManager { get; set; }

        public BlogAuthorization(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, BlogModel resource)
        {
            ApplicationUser u = await UserManager.GetUserAsync(context.User);
            if ((requirement.Name == Operations.Modify.Name || requirement.Name == Operations.Delete.Name) && u == resource.Author)
                context.Succeed(requirement);
        }
    }
}
