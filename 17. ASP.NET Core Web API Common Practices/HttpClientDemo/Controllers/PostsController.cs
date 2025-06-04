using System.Text;
using System.Text.Json;
using HttpClientDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(IHttpClientFactory httpClientFactory) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var httpClient = httpClientFactory.CreateClient();
        // var httpRequestMessage = new HttpRequestMessage
        // {
        //     Method = HttpMethod.Get,
        //     RequestUri = new Uri("https://jsonplaceholder.typicode.com/posts")
        // };
        // var response = await httpClient.SendAsync(httpRequestMessage);
        // response.EnsureSuccessStatusCode();
        // var content = await response.Content.ReadAsStringAsync();

        var content = await httpClient.GetStringAsync("https://jsonplaceholder.typicode.com/posts");
        var posts = JsonSerializerHelper.DeserializeWithCamelCase<List<Post>>(content);

        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
        // this code is commented because we can set the base address of the HttpClient instance when registering the HttpClient instance.
        // var httpClient = httpClientFactory.CreateClient();
        // var response = await httpClient.GetAsync($"https://jsonplaceholder.typicode.com/posts/{id}");
        var httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        var response = await httpClient.GetAsync($"posts/{id}");
        var content = await response.Content.ReadAsStringAsync();
        var post = JsonSerializerHelper.DeserializeWithCamelCase<Post>(content);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(Post post)
    {
        var httpClient = httpClientFactory.CreateClient();
        var json = JsonSerializerHelper.SerializeWithCamelCase(post);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", data);
        var content = await response.Content.ReadAsStringAsync();
        var createdPost = JsonSerializerHelper.DeserializeWithCamelCase<Post>(content);
        return Ok(createdPost);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, Post post)
    {
        var httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        var json = JsonSerializerHelper.SerializeWithCamelCase(post);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"posts/{id}", data);
        var content = await response.Content.ReadAsStringAsync();
        var updatedPost = JsonSerializerHelper.DeserializeWithCamelCase<Post>(content);
        return Ok(updatedPost);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.DeleteAsync($"posts/{id}");
        response.EnsureSuccessStatusCode();
        return NoContent();
    }
}