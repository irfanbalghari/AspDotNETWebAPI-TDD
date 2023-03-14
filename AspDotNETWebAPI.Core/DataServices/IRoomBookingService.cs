using AspDotNETWebAPI.Core.Domain;

namespace AspDotNETWebAPI.Core.DataServices
{
    public interface IRoomBookingService
	{
		void Save(RoomBooking roomBooking);
		IEnumerable<Room> GetAvailableRooms(DateTime date);

	}
}
