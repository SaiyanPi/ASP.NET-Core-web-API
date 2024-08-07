using SecondApi2.Models;


// This is the step 1 of implementing Constructor Injection.
//  as stated, INTERFACE(ABSTRACTION) and its implementation is created .
//  PostService.cs has the implementation of this Interface
namespace SecondApi2.Services;

public interface IPostService
{
    Task CreatePost(Post item);
    Task<Post?> UpdatePost(int id, Post item);
    Task<Post?> GetPost(int id);
    Task<List<Post>> GetAllPosts();
    Task DeletePost(int id);
}