using System.Net;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.RateLimiting;
using MiddlewareDemo;
using MiddlewareDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// built-in middleware/rate-limiting middleware:
builder.Services.AddRateLimiter(_ =>
_.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueProcessingOrder = QueueProcessingOrder.
        OldestFirst;
        options.QueueLimit = 2;
    }));

// built-in middleware/request timeout middleware:
// builder.Services.AddRequestTimeouts();
// built-in middleware/request timeout middleware: configuring with policy
builder.Services.AddRequestTimeouts(option =>
 {
 option.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(5) };
 option.AddPolicy("ShortTimeoutPolicy", TimeSpan.FromSeconds(2));
 option.AddPolicy("LongTimeoutPolicy", TimeSpan.FromSeconds(10));
 });
 




var app = builder.Build();

//  simple middleware/terminal middleware
// app.Run(async context =>
// {
//     await context.Response.WriteAsync("Hello, World!");
// });
// app.Use(async (context, next) =>
// {
//     var logger = app.Services.GetRequiredService<ILogger<Program>>();
//     logger.LogInformation($"Request Host: {context.Request.Host}");
//     logger.LogInformation("My Middleware - Before");
//     await next(context);
//     logger.LogInformation("My Middleware - After");
//     logger.LogInformation($"Response StatusCode: {context.Response.StatusCode}");
// });
// app.Use(async (context, next) =>
// {
//  var logger = app.Services.GetRequiredService<ILogger<Program>>();
//  logger.LogInformation($"ClientName HttpHeader in Middleware 1:{context.Request.Headers["ClientName"]}");
//  logger.LogInformation($"Add a ClientName HttpHeader in Middleware 1");
//  context.Request.Headers.TryAdd("ClientName", "Windows");
//  logger.LogInformation("My Middleware 1 - Before");
//  await next(context);
//  logger.LogInformation("My Middleware 1 - After");
//  logger.LogInformation($"Response StatusCode in Middleware 1:{context.Response.StatusCode}");
// });
// app.Use(async (context, next) =>
// {
//  var logger = app.Services.GetRequiredService<ILogger<Program>>();
//  logger.LogInformation($"ClientName HttpHeader in Middleware 2:{context.Request.Headers["ClientName"]}");
//  logger.LogInformation("My Middleware 2 - Before");
//  context.Response.StatusCode = StatusCodes.Status202Accepted;
//  await next(context);
//  logger.LogInformation("My Middleware 2 - After");
//  logger.LogInformation($"Response StatusCode in Middleware 2:{context.Response.StatusCode}");
// });

//  lottery example:
//  1)  map the /lottery request path to a sub-request pipeline.
// app.Map("/lottery", app =>
// {
//     //  i) generate a lucky number(generated only once when the application starts.)
//     var random = new Random();
//     var luckyNumber = random.Next(1, 6);
//     //  ii) executes only when the request has a query string
//     app.UseWhen(context => context.Request.QueryString.Value == $"?{luckyNumber.ToString()}", app =>
//     {
//         app.Run(async context =>
//         {
//             await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
//         });
//     });
//      //  iii) executes when the request does not have a query string
//     app.UseWhen(context => string.IsNullOrWhiteSpace(context.Request.QueryString.Value), app =>
//     {
//         //  a) generates a random number and adds it to the HTTP header, 
//         //  then passes it to the second sub-middleware.
//         app.Use(async (context, next) =>
//         {
//             var number = random.Next(1, 6);
//             context.Request.Headers.TryAdd("number", number.ToString());
//             await next(context);
//         });
//         //  b) check the HTTP headers of the request. If the HTTP header contains the lucky number,
//         //   it uses app.Run() to write the response. This branch is done.
//         app.UseWhen(context => context.Request.Headers["number"] == luckyNumber.ToString(), app =>
//         {
//             app.Run(async context =>
//             {
//             await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
//             });
//         });
//     });
//     //  iv) writes the response to the client and shows the number that the client has chosen.
//     //  Note that if the user already got the lucky number, this part will not be executed.
//     app.Run(async context =>
//     {
//         var number = "";
//         if (context.Request.QueryString.HasValue)
//         {
//             number = context.Request.QueryString.Value?.Replace("?", "");
//         }
//         else
//         {
//             number = context.Request.Headers["number"];
//         }
//         await context.Response.WriteAsync($"Your number is {number}. Try again!");
//     });
// });
// //  2) 
// app.Run(async context =>
// {
//     await context.Response.WriteAsync($"Use the /lottery URL to play.You can choose your number with the format /lottery?1.");
// });


//APP.MAPWHEN() VS APP.USEWHEN():
//  app.UseWhen():
// app.UseWhen(context => context.Request.Query.ContainsKey("branch"),
// app =>
// {
//     app.Use(async (context, next) =>
//     {
//         var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
//         logger.LogInformation($"From UseWhen(): Branch used = {context.Request.Query["branch"]}");
//         await next();
//     });
//     app.Run(async context =>
//     {
//         await context.Response.WriteAsync("Hello from the branched pipeline");
//     });
// });
// app.Run(async context =>
// {
// await context.Response.WriteAsync("Hello world 1!");
// });

// app.MapWhen():
// app.MapWhen(context => context.Request.Query.ContainsKey("branches"),
// app =>
// {
//     app.Use(async (context, next) =>
//     {
//         var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
//         logger.LogInformation($"From MapWhen(): Branch used = {context.Request.Query["branches"]}");
//         await next();
//     });
//     app.Run(async context => 
//     {
//     await context.Response.WriteAsync("Hello world from inside of MapWhen");
//     });
// });
// app.Run(async context =>
// {
// await context.Response.WriteAsync("Hello world 2!");
// });


// built-in middleware/rate-limiting middleware:
 app.UseRateLimiter();
// app.MapGet("/rate-limiting-mini", () => 
//     Results.Ok($"Hello {DateTime.Now.Ticks.ToString()}")).RequireRateLimiting("fixed");

// built-in middleware/request timeout middleware:
 app.UseRequestTimeouts();

 // short-circuit middleware:
//  app.MapGet("robots.txt", () => Results.Content("User-agent: *\nDisallow: /", "text/plain"))
//  .ShortCircuit();
// another way to use short-circuit middleware:
app.MapShortCircuit((int)HttpStatusCode.NotFound, "robots.txt", "favicon.ico");

// custom middleware: (CorrelationIdMiddleware.cs)
app.UseCorrelationId();


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
