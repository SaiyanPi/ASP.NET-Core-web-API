using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
 builder.Services.AddSwaggerGen(c =>
{
    // The below line is optional. It is used to describe the API.
    // c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyBasicWebApiDemo", Version = "v1" });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, 
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});


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
