namespace ParkingSystemAPI.DTO
{
    public class ActivityDTO
    {

        public ActivityDTO(int id, string ticketNumber)
        {
            Id = id;
            TicketNumber = ticketNumber;
        }

        public int Id { get; set; }
        public string TicketNumber { get; set; }
    }
}
