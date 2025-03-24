using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Queries;
public class Queryi

{
    public async Task<List<Teacher>> GetTeachers([Service] AppDbContext context) =>
        await context.Teachers.ToListAsync();

    // public async Task<List<Teacher>> GetTeachers([Service] AppDbContext context) =>
    //     await context.Teachers.Include(x => x.Department).ToListAsync();
    
    public async Task<Teacher?> GetTeacher(Guid id, [Service] AppDbContext context) =>
        await context.Teachers.FindAsync(id);
}