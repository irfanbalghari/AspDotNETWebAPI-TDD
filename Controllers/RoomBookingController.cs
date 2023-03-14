using AspDotNETWebAPI.Core.Models;
using AspDotNETWebAPI.Core.Processors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspDotNETWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoomBookingController : ControllerBase
	{
		private IRoomBookingProcessor _roomBookingProcessor;  
		public RoomBookingController(IRoomBookingProcessor roomBookingProcessor)
		{
			_roomBookingProcessor = roomBookingProcessor;
		}

		[HttpPost]
		public async Task<ActionResult> BookRoom(RoomBookingRequest request)
		{
			if (ModelState.IsValid)
			{
				var result =  _roomBookingProcessor.BookRoom(request);
				if (result.Flag == Enums.BookingResultFlag.Success)
				{
				return Ok(result);
				}
				ModelState.AddModelError(nameof(result.Date), "No available rooms");
			}
			return BadRequest(ModelState);
		}
	}
}
