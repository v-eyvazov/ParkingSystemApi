using ParkingSystemAPI.Data;
using ParkingSystemAPI.DTO;
using ParkingSystemAPI.Models;

namespace ParkingSystemAPI.Services.Impl
{
    public class TicketService : ITicketService
    {

        private readonly ApplicationDbContext _db;

        public TicketService(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActivityDTO Save()
        {
            ParkingLot reserveLot = _db.ParkingLots.Where(x => x.IsAvailable).ToList().First();
            reserveLot.IsAvailable = false;

            Activity activity = new()
            {
                Action = "RESERVE",
                ParkingLot = reserveLot
            };

            _db.ParkingLots.Update(reserveLot);
            _db.Activities.Add(activity);
            _db.SaveChanges();

            return new ActivityDTO(activity.Id, activity.TicketNumber.ToString());
        }
    }
}
