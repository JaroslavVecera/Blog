using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime LastChange { get; set; }
    }
}
