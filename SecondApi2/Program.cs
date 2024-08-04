using SecondApi2.Services;

var builder = WebApplication.CreateBuilder(args); // this is the entry point of the application.

//  this is the step 2 of implementing CI.
//  as stated, we have now registered the INTERFACE(IPostService) and its implementation(PostService)
//  to the service container
//  and this plays one of the 4 roles of DI which is 'Injector'.

//  DI Lifetimes:
//  1. Transient =>
//      - transient service is created each time it is requested and disposed of at the end of
//        the request.
//  2. Scoped =>
//      - in web application, service means a request(connection). scoped service is created once
//        per client request and disposed of at the end of the request.
//  3. Singleton =>
//      - a service is created the first time it is requested or when providing the implementation
//        instance to the service container. and all subsequent requests will use the same instance.

builder.Services.AddScoped<IPostService, PostsService>(); 
builder.Services.AddSingleton<IDemoService, DemoService>(); 


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
