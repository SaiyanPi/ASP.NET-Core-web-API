using System.Text.Json.Serialization;
using ConcurrencyConflictDemo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SampleDbContext>();

// logging DbUpdateConcurrencyException
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Reset the database
// when you run the application, the database will be reset, and the Inventory property of product 1 
// will be set to 15.
using var scope = builder.Services.BuildServiceProvider().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
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
