using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class UserModel
    {
        public string AboutMe { get; set; }
        public DateTime Birth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LiveIn { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<BlogModel> Blogs { get; set; }
    }
}
