using Microsoft.EntityFrameworkCore;
using ParkingSystemAPI.Data;
using ParkingSystemAPI.DTO;
using ParkingSystemAPI.Exceptions;
using ParkingSystemAPI.Models;
using ParkingSystemAPI.Repositories;

namespace ParkingSystemAPI.Services.Impl
{
    public class TicketService : ITicketService
    {

        private readonly ApplicationDbContext _db;
        private readonly IParkingLotRepository _activityRepository;

        public TicketService(ApplicationDbContext db, IParkingLotRepository activityRepository)
        {
            _db = db;
            _activityRepository = activityRepository;
        }

        public ActivityDTO ReserveParkingLot()
        {
            ParkingLot reserveLot = _activityRepository.GetAvailableParkingLot();
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

        public void CheckoutParkingLot(string ticketNumber)
        {
            Activity activity = _db.Activities.Where(t => t.TicketNumber == new Guid(ticketNumber)).Include(p => p.ParkingLot).ToList().First();

            // If null, gonna throw exception on the statement above
            ParkingLot parkingLot = activity.ParkingLot!;

            if (parkingLot.IsAvailable)
            {
                throw new AlreadyAvailableException();
            }

            parkingLot.IsAvailable = true;


            Activity checkout = new()
            {
                Action = "CHECKOUT",
                ParkingLot = activity.ParkingLot
            };

            _db.Activities.Add(checkout);
            _db.ParkingLots.Update(parkingLot);
            _db.SaveChanges();

        }

        public void CancelParkingLot(string ticketNumber)
        {
            Activity activity = _db.Activities.Where(t => t.TicketNumber == new Guid(ticketNumber)).Include(p => p.ParkingLot).ToList().First();

            // If null, gonna throw exception on the statement above
            ParkingLot parkingLot = activity.ParkingLot!;

            if (parkingLot.IsAvailable)
            {
                throw new AlreadyAvailableException();
            }

            parkingLot.IsAvailable = true;


            Activity checkout = new()
            {
                Action = "CANCEL",
                ParkingLot = activity.ParkingLot
            };

            _db.Activities.Add(checkout);
            _db.ParkingLots.Update(parkingLot);
            _db.SaveChanges();
        }
    }
}
