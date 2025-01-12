using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClaimBasedAuthorizationDemo.IntegrationTest
{
    public class AuthTests(IntegrationTestsFixture fixture) : IClassFixture<IntegrationTestsFixture>
    {
        // for anonymous endpoint
        [Fact]
        public async Task GetAnonymousWeatherForecast_ShouldReturnOk()
        {
            // Arrange
            var client = fixture.CreateClient();
            // Act
            var response = await client.GetAsync("/weatherforecast/anonymous");
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
            // Deserialize the response
            var responseContent = await response.Content.ReadAsStringAsync();
            var weatherForecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            weatherForecasts.Should().NotBeNull();
            weatherForecasts.Should().HaveCount(5);
        }

        // sad path for Get()
        [Fact]
        public async Task GetWeatherForecast_ShouldReturnUnauthorized_WhenNotAuthorized()
        {
            // Arrange
            var client = fixture.CreateClient();
            // Act
            var response = await client.GetAsync("/weatherforecast");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        // second approach mentioned in note
        [Fact]
        public async Task GetWeatherForecast_ShouldReturnOk_WhenAuthorized()
        {
            // Arrange
            var token = fixture.GenerateToken("TestUser");
            var client = fixture.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // Act
            var response = await client.GetAsync("/weatherforecast");
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
            // Deserialize the response
            var responseContent = await response.Content.ReadAsStringAsync();
            var weatherForecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            weatherForecasts.Should().NotBeNull();
            weatherForecasts.Should().HaveCount(5);
        }

        // happy path
        [Fact]
        public async Task GetDrivingLicense_ShouldReturnOk_WhenAuthorizedWithTestAuthHandler()
        {
            // Arrange
            //var client = fixture.WithWebHostBuilder(builder =>
            //{
            //    builder.ConfigureTestServices(services =>
            //    {
            //        services.AddAuthentication(options =>
            //        {
            //            options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
            //            options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            //            options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
            //        }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
            //    });
            //}).CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
            //client.DefaultRequestHeaders.Add(TestAuthHandler.UserNameHeader, "saiyan");
            //client.DefaultRequestHeaders.Add(TestAuthHandler.CountryHeader, "Nepal");
            //client.DefaultRequestHeaders.Add(TestAuthHandler.AccessNumberHeader, "12345678");
            //client.DefaultRequestHeaders.Add(TestAuthHandler.DrivingLicenseNumberHeader, "0123456789");

            var client = fixture.CreateClientWithAuth("saiyan", "Nepal", "12345678", "0123456789");
            // Act
            var response = await client.GetAsync("/weatherforecast/driving-license");
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
        }

        // sad path
        [Fact]
        public async Task GetDrivingLicense_ShouldReturnForbidden_WhenRequiredClaimsNotProvidedWithTestAuthHandler()
        {
            // Arrange
            //var client = fixture.WithWebHostBuilder(builder =>
            //{
            //    builder.ConfigureTestServices(services =>
            //    {
            //        services.AddAuthentication(options =>
            //        {
            //            options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
            //            options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            //            options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
            //        }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
            //    });
            //}).CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
            //client.DefaultRequestHeaders.Add(TestAuthHandler.UserNameHeader, "saiyan");
            //client.DefaultRequestHeaders.Add(TestAuthHandler.CountryHeader, "Nepal");
            //client.DefaultRequestHeaders.Add(TestAuthHandler.AccessNumberHeader, "12345678");
            // //client.DefaultRequestHeaders.Add(TestAuthHandler.DrivingLicenseNumberHeader, "0123456789");

            var client = fixture.CreateClientWithAuth("saiyan", "Nepal", "12345678","");
            // Act
            var response = await client.GetAsync("/weatherforecast/driving-license");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            //response.EnsureSuccessStatusCode(); // Status Code 200-299
            //response.Content.Headers.ContentType.Should().NotBeNull();
            //response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
        }
    }
}
