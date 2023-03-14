using AspDotNETWebAPI.Controllers;
using AspDotNETWebAPI.Core.Models;
using AspDotNETWebAPI.Core.Processors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspDotNETWebAPI.Tests
{
	public class RoomBookingControllerTests
	{
		private Mock<IRoomBookingProcessor> _roomBookingProcessor;
		private RoomBookingController _controller;
		private RoomBookingRequest _request;
		private RoomBookingResult _result;
		public RoomBookingControllerTests()
		{
			_roomBookingProcessor = new Mock<IRoomBookingProcessor>();
			_controller = new RoomBookingController(_roomBookingProcessor.Object);
			_request = new RoomBookingRequest();
			_result = new RoomBookingResult();
			_roomBookingProcessor.Setup(x => x.BookRoom(_request)).Returns(_result);
		}

		[Theory]
		[InlineData(1, true, typeof(OkObjectResult))]
		[InlineData(0, false, typeof(BadRequestObjectResult))]
		public async Task Should_Call_Booking_Method_When_Valid(int expectedMethodCall, bool isModelValid, Type expectedActionResultType)
		{
			// arrange
			if (!isModelValid)
			{
				_controller.ModelState.AddModelError("Key", "Error Message");
			}
			// act 
			var result = await _controller.BookRoom(_request);
			// assert
			result.ShouldBeOfType(expectedActionResultType);
			_roomBookingProcessor.Verify(x => x.BookRoom(_request), Times.Exactly(expectedMethodCall));
		}
	}
}
