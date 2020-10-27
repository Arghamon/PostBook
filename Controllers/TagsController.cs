using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PostBook.Domains;
using PostBook.Services;

namespace PostBook.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : Controller
    {


        private readonly IPostService _postService;

        public TagsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{tagName}")]

        public async Task<IActionResult> Get([FromRoute] string tagName)
        {
            Tag tag = await _postService.GetTagByNameAsync(tagName);

            if (tag == null)
                return NotFound();

            return Ok(tag);
        }
    }
}
