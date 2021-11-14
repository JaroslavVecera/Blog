using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class ViewBlogModel
    {
        public BlogModel Blog { get; set; }
        public CommentModel Comment { get; set; }
    }
}
