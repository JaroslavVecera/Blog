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

        public ProfileManager(UserManager<ApplicationUser> User, ProfileService profileService, IAuthorizationService authorizationService)
        {
            UserManager = User;
            ProfileService = profileService;
            AuthorizatoinService = authorizationService;
        }

        public ActionResult<ProfileModel> GetViewModel(string id, ClaimsPrincipal principals)
        {
            var user = ProfileService.GetProfile(id);
            if (user == null)
                return new NotFoundResult();
            return new ProfileModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
