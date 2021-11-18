using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class ProfileModel
    {
        public string UserId { get; set; }
        public string AboutMe { get; set; }
        public DateTime Birth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LiveIn { get; set; }
        public List<CommentLinkModel> LatestComments { get; set; } = new List<CommentLinkModel>();
        public List<BlogModel> LatestBlogs { get; set; } = new List<BlogModel>();
    }
}
