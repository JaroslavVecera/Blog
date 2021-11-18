using System;

namespace Blog.Models
{
    public class CommentLinkModel
    {
        public CommentModel Comment { get; set; }
        public int BlogId { get; set; }
    }
}
