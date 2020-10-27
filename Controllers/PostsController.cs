using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PostBook.Contracts.Requests;
using PostBook.Contracts.Responses;
using PostBook.Domains;
using PostBook.Extensions;
using PostBook.Services;

namespace PostBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }


        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetPostsAsync());
        }

        // GET: api/posts/{id}
        [HttpGet("{postId}")]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        // PUT: api/posts/{id}
        [HttpPut("{postId}")]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {

            var userOwnsPost = await _postService.UserOwnsPost(postId, HttpContext.GetUserId());

            Console.WriteLine(userOwnsPost);

            if (!userOwnsPost)
            {
                return BadRequest(new { Error = "You don't own this post" });
            }

            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post);

            if (updated)
                return Ok(post);

            return NotFound();
        }

        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {

            var postId = Guid.NewGuid();
            var tagId = Guid.NewGuid();

            var post = new Post
            {
                Name = request.Name,
                UserId = HttpContext.GetUserId(),
                PostTags = request.Tags.Select(x => new PostTag { PostId = postId, TagName = x }).ToList()
            };

            await _postService.CreatePostAsync(post);

            //var response = new PostResponse { Id = post.Id, Name = post.Name, Tags = post.PostTags };

            var baseUrl = $"{HttpContext.Request.Scheme}//{HttpContext.Request.Host.ToUriComponent()}";
            var locarionUri = baseUrl + "/api/posts/" + post.Id.ToString();

            return Created(locarionUri, post);
        }

        // DELETE: api/posts/{id}
        [HttpDelete("{postId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {

            var userOwnsPost = await _postService.UserOwnsPost(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { Error = "You don't own this post" });
            }

            var deleted = await _postService.DeletePostAsync(postId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpGet("by/{tagName}")]
        public async Task<IActionResult> GetPostsByTag([FromRoute] string tagName)
        {
            var posts = await _postService.GetPostByTag(tagName);
            Console.WriteLine(posts);
            return Ok(posts);
        }
    }
}
