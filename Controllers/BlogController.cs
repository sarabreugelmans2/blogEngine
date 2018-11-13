using System;
using System.Linq;
using blogEngine.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace blogEngine.Controllers
{
    public class BlogController : Controller{

        private BloggingContext _bloggingContext;

        public BlogController(BloggingContext bloggingContext)
        {
        _bloggingContext = bloggingContext;
        }

    public IActionResult Index(){
    
      var blogs = _bloggingContext.Blogs.ToList();
      var listModel= new BlogPostList();
      listModel.Posts=blogs;
      return View(listModel);
    }
    
    [HttpGet("Blog/Edit/{blogId}")]
        public IActionResult Edit([FromRoute]int blogId)
        {
            var blog = _bloggingContext.Blogs.Find(blogId);
            var author = _bloggingContext.Author.ToList();

            BlogPost blogPost = new BlogPost()
            {
                Id = blogId,
                Title=blog.Title,
                Content=blog.Content,
                CreatedAt= blog.CreatedAt
            };

            var authorList = new AuthorList();
            authorList.Authors = author;

            BlogAuthorViewModel BAVM = new BlogAuthorViewModel();
            BAVM.Blog=blogPost;
            BAVM.AuthorList=authorList;
            
            return View(BAVM);
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
            return View(BCVM);
        }

       
     
       public BlogPost GetBlogPost( int blogId)
        {
            var blog = _bloggingContext.Blogs.Find(blogId);
           BlogPost bModel = new BlogPost() {
                Id = blogId,
                Title=blog.Title,
                Author_id=blog.Author_id,
                Content=blog.Content,
                CreatedAt= blog.CreatedAt
            };
            return bModel;
        }
         public CommentList GetCommentModel(int blogId)
        {
            var comments = _bloggingContext.Comment.Where(_bloggingContext => _bloggingContext.Blog_id == blogId).ToList();
            var listModel= new CommentList();
            listModel.Comments=comments;
            return listModel;
        }
        public Author GetAuthorModel( int AuthorId)
        {
            var Author = _bloggingContext.Author.Find(AuthorId);
            Author aModel = new Author() {
                Id = AuthorId,
                Name=Author.Name
            };
            return aModel;
        }
       
    

        [HttpPost]
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
    public IActionResult CreatePost(BlogPost model)
            {
            var blog = new BlogPost {
                Title=model.Title,
                Author_id=model.Author_id,
                Content=model.Content,
                CreatedAt= DateTime.Now };
            _bloggingContext.Blogs.Add(blog);
            _bloggingContext.SaveChanges();
            return RedirectToAction("Index","Blog");
                
            } 
        public IActionResult Create()
        {

            return View();
                
        }    
        
        [HttpGet("Blog/Detele/{blogId}")]
        public IActionResult Delete([FromRoute]int blogId)
        {
            _bloggingContext.Remove(_bloggingContext.Blogs.Find(blogId));
            _bloggingContext.SaveChanges();
            return RedirectToAction("Index","Blog");
        }

         [HttpPost]
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
            return RedirectToAction("Index","Blog");
                
            } 
    }
    
}
    