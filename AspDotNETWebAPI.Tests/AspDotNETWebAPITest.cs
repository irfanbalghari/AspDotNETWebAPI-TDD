using AspDotNETWebAPI.Controllers;
using AspDotNETWebAPI.Core.DataServices;
using AspDotNETWebAPI.Core.Domain;
using AspDotNETWebAPI.Core.Models;
using AspDotNETWebAPI.Core.Processors;
using AspDotNETWebAPI.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace AspDotNETWebAPI.Tests
{
	public class AspDotNETWebAPITest
	{
		private RoomBookingProcessor _processor;
		private RoomBookingRequest _request;
		private Mock<IRoomBookingService> _roomBookingServiceMock;
		private List<Room> _availableRooms;
		public AspDotNETWebAPITest()
		{

			//Arrange
			_request = new RoomBookingRequest
			{
				FullName = "balghari",
				Email = "balghari@gmail.com",
				Date = new DateTime()
			};
			_availableRooms = new List<Room>() { new Room() { Id = 1 } };
			_roomBookingServiceMock = new Mock<IRoomBookingService>();
			_roomBookingServiceMock.Setup(r => r.GetAvailableRooms(_request.Date))
				.Returns(_availableRooms);
			_processor = new RoomBookingProcessor(_roomBookingServiceMock.Object);

		}
		[Fact]
		public void Should_Return_WeatherForecast()
		{
			var loggerMock = new Mock<ILogger<WeatherForecastController>>();
			var controller = new WeatherForecastController(loggerMock.Object);

			var result = controller.Get();
			result.Count().ShouldBeGreaterThan(1);
			result.ShouldNotBeNull();
		}

		[Fact]
		public void Should_Return_Room_Booking_Response_With_Request_Values()
		{
			// Act
			RoomBookingResult result = _processor.BookRoom(_request);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(_request.FullName, result.FullName);
			Assert.Equal(_request.Date, result.Date);
			Assert.Equal(_request.Email, result.Email);
		}

		[Fact]
		public void Should_Throw_exception_for_Null_Request()
		{
			var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
			exception.ParamName.ShouldBe("request");
		}

		[Fact]
		public void Should_Save_Room_Booking_Request()
		{
			RoomBooking savedBooking = null;
			_roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
				.Callback<RoomBooking>(booking =>
			{
				savedBooking = booking;
			});
			_processor.BookRoom(_request);

			_roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Once);
			savedBooking.ShouldNotBeNull();
			savedBooking.FullName.ShouldBe(_request.FullName);
			savedBooking.Date.ShouldBe(_request.Date);
			savedBooking.Email.ShouldBe(_request.Email);
			savedBooking.RoomId.ShouldBe(_availableRooms.First().Id);
		}

		[Fact]
		public void Should_Not_Save_Room_Booking_Request_If_None_Available()
		{
		    _availableRooms.Clear();
			_processor.BookRoom(_request);

			_roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
			
		}
		[Theory]
		[InlineData(BookingResultFlag.Failure, false)]
		[InlineData(BookingResultFlag.Success, true)]
		public void Should_Return_SuccessFailure_Flag_In_Result(BookingResultFlag bookingResultFlag , bool isAvailable)
		{
			if(!isAvailable)
			{
				_availableRooms.Clear();
			}
			var result = _processor.BookRoom(_request);
			bookingResultFlag.ShouldBe(result.Flag);
		}
		[Theory]
		[InlineData (1, true)]
		[InlineData (null, false)]
		public void Should_Return_Room_BookingId_In_Result(int? roomBookingId, bool isAvailable)
		{
			if(!isAvailable)
			{
				_availableRooms.Clear();
			} 
			else
			{
			_roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>()))
				.Callback<RoomBooking>(booking =>
				{
					booking.Id = roomBookingId.Value;
				});
			}
			var result = _processor.BookRoom(_request);
			//roomBookingId.ShouldBe();
			result.roomBookingId.ShouldBe(roomBookingId);

		}
	}
}