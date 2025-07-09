using System.Threading.RateLimiting;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("PollyServerWebApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5062");
});

// Add a timeout policy globally
builder.Services.AddResiliencePipeline("timeout-5s-pipeline", configure =>
{
    configure.AddTimeout(TimeSpan.FromSeconds(5));
});

//// Add a rate-limit policy
// builder.Services.AddResiliencePipeline("rate-limit-5-requests-in-3-seconds", configure =>
// {
//     configure.AddRateLimiter(new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
//     { PermitLimit = 5, Window = TimeSpan.FromSeconds(3) }));

//     // configure.AddRateLimiter(new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
//     // { PermitLimit = 100, Window = TimeSpan.FromMinutes(1) }));

// });
builder.Services.AddResiliencePipeline("rate-limit-5-requests-in-3-seconds", (configure, context) =>
{
    var rateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
    { PermitLimit = 5, Window = TimeSpan.FromSeconds(3) });
    configure.AddRateLimiter(rateLimiter);

    // Dispose the rate limiter when the pipeline is disposed
    context.OnPipelineDisposed(() => rateLimiter.Dispose());
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
