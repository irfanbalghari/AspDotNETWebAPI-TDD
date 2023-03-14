using AspDotNETWebAPI.Core.DataServices;
using AspDotNETWebAPI.Core.Domain;
using AspDotNETWebAPI.Core.Domain.BaseModel;
using AspDotNETWebAPI.Core.Models;
using AspDotNETWebAPI.Enums;

namespace AspDotNETWebAPI.Core.Processors
{
	public class RoomBookingProcessor : IRoomBookingProcessor
	{
		private IRoomBookingService _roomBookingService;
		public RoomBookingProcessor(IRoomBookingService roomBookingService)
		{
			_roomBookingService = roomBookingService;
		}

		public RoomBookingResult BookRoom(RoomBookingRequest request)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}
			var availableRooms = _roomBookingService.GetAvailableRooms(request.Date);
			var result = CreateBookingObject<RoomBookingResult>(request);
			if (availableRooms.Any())
			{
				var room = availableRooms.First();
				var roomBooking = CreateBookingObject<RoomBooking>(request);
				roomBooking.RoomId = room.Id;
				_roomBookingService.Save(roomBooking);
				result.roomBookingId = roomBooking.Id;
				result.Flag = BookingResultFlag.Success;
			}
			else { 
				result.Flag = BookingResultFlag.Failure;
			}

			return result;
		}
		private TBooking CreateBookingObject<TBooking>(RoomBookingRequest request)
			where TBooking : RoomBookingBase, new()
		{
			return new TBooking
			{
				FullName = request.FullName,
				Date = request.Date,
				Email = request.Email
			};
		}
	}
}