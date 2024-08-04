using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SecondApi.Models;
using SecondApi.Services;

//  HERE, CONTROLLER IS THE HIGH-LEVEL MODULE AND CURRENTLY IT DEPENDS ON THE LOW-LEVEL MODULE(PostService)
//  IF WE CHANGE SERVICE, THE CONTROLLER MUST BE CHANGED AS WELL SO THIS IS NOT DI
//  DI STATES THAT the high level module should not depend on low level module, instead both should
//  depend on ABSTRACTIONS that expose the behaviour needed by high level module.
//  if we invert this dependency relationships by creating an interface for SERVICE, 
//  both SERVICE & CONTROLLER will depend on the INTERFACE.
//  see SecondApi2 where DI is implemented.
namespace SecondApi.Controllers
{
    // indicates the URL of the controller
    // [controller] is like a placeholder, which will be replaced with the name of the controller in the routing
    // so the route of this controller is /api/Posts
    [Route("api/[controller]")] 

    // indicates that the controller is a web API controller
    [ApiController]
    public class PostsController : ControllerBase
    {
        // [HttpGet]
        // public ActionResult<List<Post>> GetPosts()
        // { 
        //     return new List<Post>
        //     { 
        //         new()
        //         { 
        //             UserId = 1, 
        //             Id = 1, 
        //             Title = "First Post",
        //             Body = "This is the first post content" 
        //         }, 
        //         new()
        //         { 
        //             UserId = 2, 
        //             Id = 2, 
        //             Title = "Second Post", 
        //             Body = "This is the second post content" 
        //         }
        //     }; 
        // }

        //  If the PostService class has its own dependencies, they must also be initialized 
        //  by the PostsController.
        private readonly PostsService _postsService;
        public PostsController()
        {
            _postsService = new PostsService();
        }

        //  the return type of the GetPosts() method is ActionResult<IEnumerable<Post>>.
        //  ASP.NET Core can automatically convert the object tp JSON and return it to the client in
        //  the response message. also it can return other HTTP status codes, such as NotFound,
        //  BadRequest, InternalServerError, and so on.

        [HttpGet("{id}")]
        public async Task<ActionResult<PostsController>> GetPost(int id){
            var post = await _postsService.GetPost(id);
            if(post == null){
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<PostsController>> CreatePost(Post post)
        {
            await _postsService.CreatePost(post);
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, Post post)
        {
            if(id != post.Id)
            {
                return BadRequest();
            }
            var updatedPost = await _postsService.UpdatePost(id, post);
            if (updatedPost == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            return await _postsService.GetAllPosts();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postsService.DeletePost(id);
            return NoContent();
        }
    }
}
