
using AspDotNETWebAPI.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace AspDotNETWebAPI.Tests
{
	public class WeatherControllerTests
	{
		[Fact]
		public void Should_Return_Weather_Forecast()
		{
			var loggerMock = new Mock<ILogger<WeatherForecastController>>();
			var controller = new WeatherForecastController(loggerMock.Object);
			var result = controller.Get();
			Assert.NotNull(result);
			result.Count().ShouldBeGreaterThan(0);
		}
	}
}
