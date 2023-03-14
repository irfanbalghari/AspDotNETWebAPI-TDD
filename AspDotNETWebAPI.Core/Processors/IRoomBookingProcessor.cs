
using AspDotNETWebAPI.Core.Models;

namespace AspDotNETWebAPI.Core.Processors
{
	public interface IRoomBookingProcessor
	{
		RoomBookingResult BookRoom(RoomBookingRequest request);
	}
}
