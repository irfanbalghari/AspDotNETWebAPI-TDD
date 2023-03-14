using AspDotNETWebAPI.Core.Domain.BaseModel;
using AspDotNETWebAPI.Enums;

namespace AspDotNETWebAPI.Core.Models
{
	public class RoomBookingResult : RoomBookingBase
	{
		public int? roomBookingId { get; set; }

		public BookingResultFlag Flag { get; set; }
	}
}