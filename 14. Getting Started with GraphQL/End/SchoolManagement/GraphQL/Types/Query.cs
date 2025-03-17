using HotChocolate.Data.Filters;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.GraphQL.Filetrs;
using SchoolManagement.GraphQL.Sorts;
using SchoolManagement.Models;
using SchoolManagement.Services;

namespace SchoolManagement.GraphQL.Types;
public class Query
{
    // public async Task<List<Teacher>> GetTeachers([Service] AppDbContext context) =>
    //     await context.Teachers.ToListAsync();

    // public async Task<List<Teacher>> GetTeachers([Service] AppDbContext context) =>
    //     await context.Teachers.Include(x => x.Department).ToListAsync();
    
    // public async Task<Teacher?> GetTeacher(Guid id, [Service] AppDbContext context) =>
    //     await context.Teachers.FindAsync(id);
    
        
    public TeacherType? Teacher { get; set; } = new();
    public List<TeacherType> Teachers { get; set; } = new();
    public List<DepartmentType> Departments {get; set; } = new();
     public DepartmentType? Department { get; set; } = new();

    // DI using the Service attribute
    // public async Task<List<Teacher>> GetTeachersWithDI([Service (ServiceKind.Resolver)] ITeacherService teacherService) =>
    //     await teacherService.GetTeachersAsync();
    public async Task<List<Teacher>> GetTeachersWithDI(ITeacherService teacherService) =>
        await teacherService.GetTeachersAsync();
    public List<SchoolRoomType> SchoolRooms { get; set; } = new();
    public List<SchoolItemType> SchoolItems { get; set; } = new();
    public List<Student> Students { get; set; } = new();

    // public List<Group> Groups { get; set; } = new();
    // for filtering in resolver manually
    public List<Student> StudentsWithCustomFilter { get; set; } = new();
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        // descriptor.Field(x => x.Teacher)
        //     .Name("teacher")
        //     .Description("This is the teacher in the school.")
        //     .Type<TeacherType>()
        //     .Argument("id", a => a.Type<NonNullType<UuidType>>())
        //     .Resolve(async context =>
        //     {
        //         // var id = context.ArgumentValue<Guid>("id");
        //         // var teacher = await context.Service<AppDbContext>().Teachers.FindAsync(id);
        //         // return teacher;
        //         var id = context.ArgumentValue<Guid>("id");
        //         var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
        //         await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        //         var teacher = await dbContext.Teachers.FindAsync(id);
        //         return teacher;
        //     });

        // The following code uses the DbContextFactory to create a new instance of the AppDbContext class.
        // It is replaced by Dipendency Injection

        // descriptor.Field(x => x.Teachers)
        //     .Name("teachers") // This configuration can be omitted if the name of the field is the same as the name of the property.
        //     .Description("This is the list of teachers in the school.")
        //     .Type<ListType<TeacherType>>()
        //     .Resolve(async context =>
        //     {
        //         // var teachers = await context.Service<AppDbContext>().Teachers.ToListAsync();
        //         // return teachers;
        //         var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
        //         await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        //         var teachers = await dbContext.Teachers.ToListAsync();
        //         return teachers;
        //     });
        
        descriptor.Field(x => x.Teachers)
            .Description("This is the list of teachers in the school.")
            .Type<ListType<TeacherType>>()
            .Resolve(async context =>
            {
                var teacherService = context.Service<ITeacherService>();
                var teachers = await teacherService.GetTeachersAsync();
                return teachers;
            });
            
        descriptor.Field(x => x.Teacher)
            .Name("teacher")
            .Description("This is the teacher in the school.")
            .Type<TeacherType>()
            .Argument("id", a => a.Type<NonNullType<UuidType>>())
            .Resolve(async context =>
            {
                var id = context.ArgumentValue<Guid>("id");
                var teacherService = context.Service<ITeacherService>();
                var teacher = await teacherService.GetTeacherAsync(id);
                return teacher;
            });

        descriptor.Field(x => x.Departments)
            .Name("departments")
            .Description("This is the list of departments in the school.")
            .Type<ListType<DepartmentType>>()
            .Resolve(async context =>
            {
                var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
                await using var dbContext = await dbContextFactory.CreateDbContextAsync();
                var departments = await dbContext.Departments.ToListAsync();
                return departments;
            });

        // descriptor.Field(x => x.Department)
        //     .Name("department")
        //     .Description("This is the department in the school.")
        //     .Type<DepartmentType>()
        //     .Argument("id", a => a.Type<NonNullType<UuidType>>())
        //     .Resolve(async context =>
        //     {
        //         var id = context.ArgumentValue<Guid>("id");
        //         var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
        //         await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        //         var department = await dbContext.Departments.FindAsync(id);
        //         return department;
        //     });

        descriptor.Field(x => x.SchoolRooms)
            .Description("This is the list of school rooms in the school.")
            .Type<ListType<SchoolRoomType>>()
            .Resolve(async context =>
            {
                var service = context.Service<ISchoolRoomService>();
                var schoolRooms = await service.GetSchoolRoomsAsync();
                return schoolRooms;
            });
        
        descriptor.Field(x => x.SchoolItems)
            .Description("This is the list of school items in the school.")
            .Type<ListType<SchoolItemType>>()
            .Resolve(async context =>
            {
                var equipmentService = context.Service<IEquipmentService>();
                var furnitureService = context.Service<IFurnitureService>();
                var equipmentTask = equipmentService.GetEquipmentListAsync();
                var furnitureTask = furnitureService.GetFurnitureListAsync();
                await Task.WhenAll(equipmentTask, furnitureTask);
                var schoolItems = new List<object>();
                schoolItems.AddRange(equipmentTask.Result);
                schoolItems.AddRange(furnitureTask.Result);
                return schoolItems;
            });

        // descriptor.Field(x => x.Groups)
        //     .Description("This is the list of groups in the school.")
        //     .Resolve(async context =>
        //     {
        //         var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
        //         var dbContext = await dbContextFactory.CreateDbContextAsync();
        //         var groups = await dbContext.Groups.ToListAsync();
        //         // var groups = dbContext.Groups.AsQueryable();
        //         return groups;
        //     });

        descriptor.Field(x => x.Students)
            .Description("This is the list of students in the school.")
            // .UsePaging()
            // .UsePaging(options: new PagingOptions()
            // {
            //     MaxPageSize = 20,
            //     DefaultPageSize = 5,
            //     IncludeTotalCount = true
            // })
            // .UseOffsetPaging()
            .UseOffsetPaging(options: new PagingOptions()
            {
            MaxPageSize = 20,
            DefaultPageSize = 5,
            IncludeTotalCount = true
            })
            // .UseFiltering()
            .UseFiltering<StudentFilterType>()
            // .UseSorting()
            .UseSorting<StudentSortType>()
            .Resolve(async context =>
            {
                var dbContextFactory = context.Service<IDbContextFactory<AppDbContext>>();
                var dbContext = await dbContextFactory.CreateDbContextAsync();
                var students = dbContext.Students.AsQueryable();
                return students;
            });

        // descriptor.Field(x => x.StudentsWithCustomFilter)
        //     .Description("This is the list of students in the school.")
        //     .UseFiltering<CustomStudentFilterType>().Resolve(async context =>
        //     {
        //         var service = context.Service<IStudentService>();
        //         // The following code uses the custom filter.
        //         var filter = context.GetFilterContext()?.ToDictionary();
        //         if (filter != null && filter.ContainsKey("groupId"))
        //         {
        //             var groupFilter = filter["groupId"]! as Dictionary<string, object>;
        //             if (groupFilter != null && groupFilter.ContainsKey("eq"))
        //             {
        //                 if (!Guid.TryParse(groupFilter["eq"].ToString(),out var groupId))
        //                 {
        //                 throw new ArgumentException("Invalid group id", nameof(groupId));
        //                 }
        //                 var students = await service.GetStudentsByGroupIdAsync(groupId);
        //                 return students;
        //             }
        //             if (groupFilter != null && groupFilter.ContainsKey("in"))
        //             {
        //                 if (groupFilter["in"] is not IEnumerable<string> groupIds)
        //                 {
        //                     throw new ArgumentException("Invalid group ids", nameof(groupIds));
        //                 }
        //                 groupIds = groupIds.ToList();
        //                 if (groupIds.Any())
        //                 {
        //                 var students = await service.GetStudentsByGroupIdsAsync(groupIds .Select(x => 
        //                     Guid.Parse(x.ToString())).ToList());
        //                     return students;
        //                 }
        //                 return new List<Student>();
        //             }
        //         }
        //         var allStudents = await service.GetStudentsAsync();
        //         return allStudents;
        //     });
    }

   
}
