using Microsoft.EntityFrameworkCore;

using SchoolManagement.Data;
using SchoolManagement.GraphQL.Mutations;
using SchoolManagement.GraphQL.Queries;
using SchoolManagement.GraphQL.Types;
using SchoolManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// registering Interface(Dependency Injection section)
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ISchoolRoomService, SchoolRoomService>();

builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IFurnitureService, FurnitureService>();
builder.Services.AddScoped<IStudentService, StudentService>();

// Register the GraphQL services
builder.Services
    .AddGraphQLServer()
    // .RegisterDbContext<AppDbContext>()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled)
    .RegisterService<ITeacherService>(ServiceKind.Resolver)   
    // .AddQueryType<Queryi>()
    .AddQueryType<QueryType>()
    .AddType<LabRoomType>()
    .AddType<ClassroomType>()
    .AddFiltering()
    .AddSorting()
    .AddMutationType<Mutation>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// map the GraphQL endpoint to expose the GraphQL schema
app.MapGraphQL();

// app.MapGraphQLVoyager();
app.MapGraphQLVoyager("/voyager");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
