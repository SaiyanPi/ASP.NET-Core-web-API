using OutputCaching.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Debug Logging
builder.Logging.AddConsole();
// register output caching
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(x => x.Cache());
    // define caching policies for different endpoints
    options.AddPolicy("Expire600", x => x.Expire(TimeSpan.FromSeconds(600))); //cached response will expire in 10 minutes.
    options.AddPolicy("Expire3600", x => x.Expire(TimeSpan.FromSeconds(3600)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

// app.UseOutputCache();

app.Run();
