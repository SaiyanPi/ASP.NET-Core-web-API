using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

//  remove all the pre-registered logging providers
builder.Logging.ClearProviders();
//  add the console logging provider
//builder.Logging.AddConsole();
//  add the windows event logging provider
//builder.Logging.AddEventLog();
//  for displaying logging message in VS 2022 output windows
//builder.Logging.AddDebug();

//  configure Serilog
var logger = new LoggerConfiguration()
    .WriteTo.File(formatter: new JsonFormatter(), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log.txt"),rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90)
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.Seq("http://localhost:5341") 
    .CreateLogger();
builder.Logging.AddSerilog(logger);

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
