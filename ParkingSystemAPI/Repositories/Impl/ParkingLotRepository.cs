using ParkingSystemAPI.Data;
using ParkingSystemAPI.Models;

namespace ParkingSystemAPI.Repositories.Impl
{
    public class ParkingLotRepository : IParkingLotRepository
    {

        private readonly ApplicationDbContext _db;

        public ParkingLotRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ParkingLot GetAvailableParkingLot()
        {
            return _db.ParkingLots.Where(x => x.IsAvailable).ToList().First();
        }
    }
}
