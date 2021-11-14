using System;

namespace Blog.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public ApplicationUser Author { get; set; }
    }
}
