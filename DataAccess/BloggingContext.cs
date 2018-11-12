using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using blogEngine.Models;


namespace blogEngine.DataAccess
{
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> config) : base(config)
    {

    }

        public DbSet<BlogPost> Blogs { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }

    public class BlogPost
    {
        //string Author beter een object, wnt als je schrijffout maakt moet je ze anders overal aanpassen
        public int Id {get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }

        
    }

    public class BlogPostList{
        public List<BlogPost> Posts { get; set; }
    }

    public class Comment
    {
        //string Author beter een object, wnt als je schrijffout maakt moet je ze anders overal aanpassen
        public int Id {get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Blog_id {get; set; }

        
    }

    public class CommentList{
        public List<Comment> Comments { get; set; }
    }

    public class BlogCommentViewModel
    {
        public BlogPost Blog { get; set; }

        public CommentList Comments { get; set; }
    }

}