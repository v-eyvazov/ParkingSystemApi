using ParkingSystemAPI.Models;

namespace ParkingSystemAPI.Repositories
{
    public interface IParkingLotRepository
    {
        public ParkingLot GetAvailableParkingLot();
    }
}
