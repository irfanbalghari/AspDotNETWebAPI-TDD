
using AspDotNETWebAPI.Core.Domain;
using AspDotNETWebAPI.Core.Models;

namespace AspDotNETWebAPI.Core.Processors
{
	public interface IRoomBookingProcessor
	{
		public RoomBookingResult BookRoom(RoomBookingRequest request);
		public List<Room> GetAllRooms();
	}
}
