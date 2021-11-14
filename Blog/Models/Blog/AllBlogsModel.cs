using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class AllBlogsModel
    {
        public List<BlogModel> Blogs { get; set; }
    }
}
