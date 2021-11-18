using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class ViewBlogModel
    {
        public BlogModel Blog { get; set; }
        public int BlogId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Comment { get; set; }
    }
}
