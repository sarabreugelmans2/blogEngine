using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogEngine.DataAccess
{
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> config) : base(config)
    {

    }

        public DbSet<BlogPost> Blogs { get; set; }
        
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


}