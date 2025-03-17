using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.GraphQL.DataLoaders;
using SchoolManagement.Models;

namespace SchoolManagement.GraphQL.Types;

public class TeacherType : ObjectType<Teacher>
{
    protected override void Configure(IObjectTypeDescriptor<Teacher> descriptor)
    {
        descriptor.Field(x => x.Department)
            .Name("department")
            .Description("This is the department to which the teacher belongs.")
            // following code is commented because we used the seperate class to define resolvers
            // .Resolve(async context =>
            // {
            //     // Following code is commented because it is replaced by the DbContextPool feature
            //     // var department = await context.Service<AppDbContext>().Departments
            //     //     .FindAsync(context. Parent<Teacher>().DepartmentId);
            //     // return department;

            //     var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
            //     await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            //     var department = await dbContext.Departments
            //         .FindAsync(context.Parent<Teacher>().DepartmentId);
            //     return department;

            // });

            // .ResolveWith<TeacherResolvers>(x => x.GetDepartment(default, default));
            .ResolveWith<TeacherResolvers>(x => x.GetDepartment(default, default, default));
    }
}

public class TeacherResolvers
{
    // following code is commented because it is replaced by the batch data loader

    // public async Task<Department> GetDepartment([Parent] Teacher teacher, 
    //     [Service] IDbContextFactory<AppDbContext> dbContextFactory)
    // {
    //     await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    //     var department = await dbContext.Departments.FindAsync(teacher.DepartmentId);
    //     return department;
    // }

    public async Task<Department> GetDepartment([Parent] Teacher teacher, 
        DepartmentByTeacherIdBatchDataLoader departmentByTeacherIdBatchDataLoader,
        CancellationToken cancellationToken)
    {
        var department = await departmentByTeacherIdBatchDataLoader.LoadAsync(teacher.DepartmentId, cancellationToken);
        return department;
    }
}
