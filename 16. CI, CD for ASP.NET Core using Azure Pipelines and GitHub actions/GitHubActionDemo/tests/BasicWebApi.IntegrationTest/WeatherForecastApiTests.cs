using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BasicWebApi.IntegrationTest
{
    public class WeatherForecastApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GetWeatherForecast_ReturnsSuccessAndCorrectContentType()
        {
            //Arrange
            var client = factory.CreateClient();

            //Act
            var response = await client.GetAsync("/WeatherForecast");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
                // Deserialize the response
            var responseContent = await response.Content.ReadAsStringAsync();
            var weatherForecast = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            weatherForecast.Should().NotBeNull();
            weatherForecast.Should().HaveCount(5);
        }

    }
}
