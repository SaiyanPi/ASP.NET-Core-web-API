using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SecondApi2.Models;
using SecondApi2.Services;

namespace SecondApi2.Controllers;

//  Controller plays one of the 4 roles of DI. which is 'Client'.
[Route("api2/[controller]")] 
[ApiController]
public class PostsController : ControllerBase
{
    // DI: there are 3 types of dependency injection
    //  1. CONSTRUCTOR INJECTION = dependencies are provided as a parameters of the client's constructor.
    //  2. SETTER INJECTION = client exposes a setter method to accept the dependency.
    //  3. INTERFACE INJECTION = dependency's interface provides an injector method that will inject
    //      the dependency into any client passed to it.
    
    //  ASP.NET Core uses CONSTRUCTOR INJECTION to request dependencies.
    //  To use it, we need to do the following:
    //  1. Define Interfaces and their implementastions. (IPostService.cs)
    //  2. Register the Interfaces and the implementations to the Service container.
    //  3. Add Services as the constructor parameter to inject the dependencies.   
    
    // DI has 4 roles:
    //  1. SERVICES
    //  2. CLIENTS
    //  3. INTERFACES and
    //  4. INJECTORS
    
    //  this is the step 3 of implementing CI
    private readonly IPostService _postsService;
    public PostsController(IPostService postsService)
    {
        _postsService = postsService;
    }

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