using ParkingSystemAPI.DTO;

namespace ParkingSystemAPI.Services
{
    public interface ITicketService
    {
        ActivityDTO CheckoutParkingLot(string ticketNumber);
        ActivityDTO ReserveParkingLot();
    }
}
