using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Queries;
public class Query
{
    public async Task<List<Teacher>> GetTeachers([Service] AppDbContext context) =>
        await context.Teachers.ToListAsync();
}