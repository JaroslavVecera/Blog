using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class CreateBlogModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} cannot be empty.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} cannot be empty.")]
        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}
