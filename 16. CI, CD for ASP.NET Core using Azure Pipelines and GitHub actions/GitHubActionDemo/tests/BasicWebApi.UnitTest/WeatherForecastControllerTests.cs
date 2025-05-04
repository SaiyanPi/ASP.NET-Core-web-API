using BasicWebApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebApi.UnitTest
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void Get_Returns_Weatherforecast()
        {
            //Arrange
            var logger = Mock.Of<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(logger);

            //Act
            var result = controller.Get();

            //Assert
            // Assert.Equal(result.Count(), 5);
            var WeatherForecast = Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(result);
            Assert.Equal(5, WeatherForecast.Count());


        }
    }
}
