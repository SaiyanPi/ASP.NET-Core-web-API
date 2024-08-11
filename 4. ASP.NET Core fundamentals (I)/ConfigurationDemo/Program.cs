using ConfigurationDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//  3) USING OPTION PATTERN/IOPTIONS<TOPTION> INTERFACE:
//  register the DatabaseOption class as a configuration object.
//  this line must be added before the 'builder.Build()' method
//builder.Services.Configure<DatabaseOption>(builder.Configuration.GetSection(DatabaseOption.SectionName));

// register named options
//builder.Services.Configure<DatabaseOptions>(DatabaseOptions.SystemDatabaseSectionName,
//    builder.Configuration.GetSection($"{DatabaseOptions.SectionName}:{DatabaseOptions.SystemDatabaseSectionName}"));
//builder.Services.Configure<DatabaseOptions>(DatabaseOptions.BusinessDatabaseSectionName,
//    builder.Configuration.GetSection($"{DatabaseOptions.SectionName}:{DatabaseOptions.BusinessDatabaseSectionName}"));

//Group options registration
builder.Services.AddConfig(builder.Configuration);

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
