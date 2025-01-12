using InvoiceApp.IntegrationTests.SeedHelper;
using InvoiceApp.WebApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApp.IntegrationTests
{
    public class IntegrationTestingDatabaseFixture : WebApplicationFactory<Program>
    {
        private const string ConnectionString = "Server=.;Database=IntegrationTestingDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //// Set up a test database
            //builder.ConfigureServices(services =>
            //{
            //    // finding the default database context
            //    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<InvoiceDbContext>));
            //    // removing the default database context
            //    services.Remove(descriptor);
            //    // adding a new database context that uses the test database
            //    services.AddDbContext<InvoiceDbContext>(options =>
            //    {
            //        options.UseSqlServer(ConnectionString);
            //    });
            //});

            //builder.ConfigureAppConfiguration((context, config) =>
            //{
            //    config.AddJsonFile("appsettings.IntegrationTest.json");
            //});
            //builder.UseEnvironment("Development");

            builder.ConfigureServices(services =>
            {
                // finding the default database context
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<InvoiceDbContext>));
                // removing the default database context
                services.Remove(descriptor);
                // adding a new database context that uses the test database
                services.AddDbContext<InvoiceDbContext>(options =>
                {
                    options.UseSqlServer(ConnectionString);
                });
                // initializing test database
                using var scope = services.BuildServiceProvider().CreateScope();
                var scopeServices = scope.ServiceProvider;
                var dbContext = scopeServices.GetRequiredService<InvoiceDbContext>();
                Utilities.InitializeDatabase(dbContext);
            });





        }
    }
}
