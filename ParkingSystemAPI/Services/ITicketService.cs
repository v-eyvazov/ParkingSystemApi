using ParkingSystemAPI.DTO;

namespace ParkingSystemAPI.Services
{
    public interface ITicketService
    {
        void CheckoutParkingLot(string ticketNumber);
        void CancelParkingLot(string ticketNumber);
        ActivityDTO ReserveParkingLot();
    }
}
