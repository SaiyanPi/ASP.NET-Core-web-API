using SecondApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();




// //  WAYS OF DECLARING LIST
// var test1 = new List<string>();
// List<string> test2 = new List<string>();

// //  WAYS OF INITIALIZING LIST
// var test11 = new List<string>{"hello", "world"};
// var test22 = new List<int>{1, 2, 3};

// //  WAYS OF DECLARING ARRAY
// string[] test3; // DECLARING ARRAY
// test3 = new string[4]; // ALLOCATE MEMORY FOR ARRAY

// string[] test33 = {"hello","world"}; // INITIALIZING ARRAY
// var test4 = new int[]{1, 2, 3}; // INITIALIZING DURING DECLARING