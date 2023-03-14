
using AspDotNETWebAPI.Core.DataServices;
using AspDotNETWebAPI.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AspDotNETWebAPI.Persistance.Repositories
{
	public class RoomBookingService : IRoomBookingService
	{
		private readonly RoomBookingAppDbContext _dbContext;
		public RoomBookingService(RoomBookingAppDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IEnumerable<Room> GetAvailableRooms(DateTime date)
		{
			//var unavailableRoom = _dbContext.RoomBookings.Where(x => x.Date ==date).Select(x=>x.RoomId).ToList();

			//return _dbContext.Rooms.Where(q => unavailableRoom.Contains(q.Id) == false).ToList();
			return _dbContext.Rooms.Where(q => !q.RoomBookings.Any(q => q.Date == date)).ToList();

		}

		public void Save(RoomBooking roomBooking)
		{
			_dbContext.Add(roomBooking);
			_dbContext.SaveChanges();
		}
	}
}
