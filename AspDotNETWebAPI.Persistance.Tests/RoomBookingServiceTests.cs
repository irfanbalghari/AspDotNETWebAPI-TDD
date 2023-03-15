
using AspDotNETWebAPI.Core.Domain;
using AspDotNETWebAPI.Core.Domain.BaseModel;
using AspDotNETWebAPI.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace AspDotNETWebAPI.Persistance.Tests
{
	public class RoomBookingServiceTests
	{
		[Fact]
		public void Should_return_available_room()
		{
			// arrange 
			var date = new DateTime(2021, 06, 09);
			var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
				.UseInMemoryDatabase("AvailableRoomTest")
				.Options;
			using var context = new RoomBookingAppDbContext(dbOptions);
			
				context.Add(new Room { Id = 1, Name = "Room 1" });
				context.Add(new Room { Id = 2, Name = "Room 2" });
				context.Add(new Room { Id = 3, Name = "Room 3" });

				context.Add(new RoomBooking { RoomId = 1, Date = date });
				context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1) });
				context.SaveChanges();

				var roomBookingService = new RoomBookingService(context);

				// Act 
				var availableRooms = roomBookingService.GetAvailableRooms(date);

				// Assert
				Assert.Equal(2, availableRooms.Count());
				Assert.Contains(availableRooms, q => q.Id == 2 );
				Assert.Contains(availableRooms, q => q.Id == 3 );
				Assert.DoesNotContain(availableRooms, q => q.Id == 1 );
			
		}
		[Fact]
		public void Should_Save_Room_Booking()
		{
			// arrange 
			var date = new DateTime(2023, 03, 14);
			var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
				.UseInMemoryDatabase("SaveRoomTest")
				.Options;
			var roomBooking = new RoomBooking { RoomId = 1, Date = date };
			using var context = new RoomBookingAppDbContext(dbOptions);
			var roomBookingService = new RoomBookingService(context);
			roomBookingService.Save(roomBooking);
			var bookings = context.RoomBookings.ToList();
			var booking = Assert.Single(bookings);
			Assert.Equal(roomBooking.Date, booking.Date);
			Assert.Equal(roomBooking.RoomId, booking.RoomId);
			//var result = roomBookingService.GetAvailableRooms(date);

			//result.Count().ShouldBe(1);

		}
		[Fact]
		public void Should_Return_all_rooms()
		{
			// arrange 
			var date = new DateTime(2023, 03, 14);
			var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
				.UseInMemoryDatabase("SaveRoomTest")
				.Options;

			var roomBooking = new RoomBooking { RoomId = 1, Date = date };
			using var context = new RoomBookingAppDbContext(dbOptions);

			context.Add(new Room { Id = 1, Name = "Room 1" });
			context.Add(new Room { Id = 2, Name = "Room 2" });
			context.Add(new Room { Id = 3, Name = "Room 3" });
			var roomBookingService = new RoomBookingService(context);
			roomBookingService.Save(roomBooking);
			var result = roomBookingService.GetAllRooms();
			
			//result.ShouldNotBeNull();
			Assert.NotNull(result);
			result.Count().ShouldBeGreaterThan(0);
		}
	}
}
