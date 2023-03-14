using AspDotNETWebAPI.Core.Domain.BaseModel;

namespace AspDotNETWebAPI.Core.Domain
{
	public class RoomBooking : RoomBookingBase
	{
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
