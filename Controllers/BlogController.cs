using System;
using System.Linq;
using blogEngine.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogEngine.Controllers
{
    public class BlogController : Controller{

        private BloggingContext _bloggingContext;

        public BlogController(BloggingContext bloggingContext)
        {
        _bloggingContext = bloggingContext;
        }
   
    [UrlFilter]
    public IActionResult Index(){
    
      var blogs = _bloggingContext.Blogs.ToList();
      var listModel= new BlogPostList();
      listModel.Posts=blogs;
      return View(listModel);
    }
    
    [HttpGet("Blog/Edit/{blogId}")]
        public IActionResult Edit([FromRoute]int blogId)
        {
            BlogPost blog = new BlogPost();
            blog = _bloggingContext.Blogs.Find(blogId);
            // _personContext.Remove(_personContext.Persons.Find(id));
            return View(blog);
        }
    

    // home/hello?raf/ceuls
        [HttpGet("Blog/Detail/{blogId}")]
        public IActionResult Detail([FromRoute]int blogId)
        {
            BlogCommentViewModel BCVM = new BlogCommentViewModel();
            BCVM.Blog= GetBlogPost(blogId);
            BCVM.Comments = GetCommentModel(blogId);
            int AuthorId = BCVM.Blog.Author_id;
            BCVM.Author = GetAuthorModel(AuthorId);
            
            var author = _bloggingContext.Author.ToList();
            AuthorList authorList = new AuthorList();
            authorList.Authors = author;
            BCVM.AuthorList = authorList;
            return View(BCVM);
        }

       
     
       public BlogPost GetBlogPost( int blogId)
        {
            BlogPost blog = new BlogPost();
            blog =_bloggingContext.Blogs.Find(blogId);
           
            return blog;
        }
         public CommentList GetCommentModel(int blogId)
        {
            var comments = _bloggingContext.Comment.Include(_bloggingContext => _bloggingContext.Author).Where(_bloggingContext => _bloggingContext.Blog_id == blogId).ToList();
            var listModel= new CommentList();
            listModel.Comments=comments;
            return listModel;
        }
        public Author GetAuthorModel( int AuthorId)
        {
            Author author = new Author();
            author = _bloggingContext.Author.Find(AuthorId);
            
            return author;
        }
       
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(BlogPost model)
        {
            
            var blog = _bloggingContext.Blogs.Find(model.Id);
                blog.Title=model.Title;
                blog.Content=model.Content;
                blog.CreatedAt= DateTime.Now; 
            _bloggingContext.Update(blog);
            _bloggingContext.SaveChanges();
            return RedirectToAction("Index","Blog");
        }
        
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BlogPost model)
        {
             if (ModelState.IsValid){
                var blog = new BlogPost {
                    Title=model.Title,
                    Author_id=model.Author_id,
                    Content=model.Content,
                    CreatedAt= DateTime.Now };
                _bloggingContext.Blogs.Add(blog);
                _bloggingContext.SaveChanges();
                
                return RedirectToAction("Index","Blog");
             }
            
           var author = _bloggingContext.Author.ToList();
            AuthorList authorList = new AuthorList();
            authorList.Authors = author;
             BlogCommentViewModel blogAuthorView = new BlogCommentViewModel();
            blogAuthorView.AuthorList = authorList;
            blogAuthorView.Blog = model;
            
            return View(blogAuthorView);
        } 
        public IActionResult Create()
        {
            var author = _bloggingContext.Author.ToList();
            AuthorList authorList = new AuthorList();
            authorList.Authors = author;

            BlogPost blogPost = new BlogPost();
            
            BlogCommentViewModel blogAuthorView = new BlogCommentViewModel();
            blogAuthorView.AuthorList = authorList;
            blogAuthorView.Blog = blogPost;
            
            return View(blogAuthorView);
                
        }    
        
       
        [HttpPost("Blog/Detail/{blogId}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute]int blogId)
        {
            _bloggingContext.Remove(_bloggingContext.Blogs.Find(blogId));
            _bloggingContext.SaveChanges();
            return RedirectToAction("Index","Blog");
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
          public IActionResult CreateComment(Comment model)
            {

            var comment = new Comment {
                Author_id=model.Author_id,
                Content=model.Content,
                Blog_id=model.Id,
                CreatedAt= DateTime.Now
            };
            _bloggingContext.Comment.Add(comment);
            _bloggingContext.SaveChanges();
            return RedirectToAction("Detail","Blog",new { id = model.Id });
                
            } 

            
    }
    
}
    