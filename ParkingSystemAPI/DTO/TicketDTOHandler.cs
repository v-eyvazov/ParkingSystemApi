using System.Text.Json.Serialization;

namespace ParkingSystemAPI.DTO
{
    public class TicketDTOHandler
    {

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TicketLocation { get; set; }



        private TicketDTOHandler(string? description, string? ticketLocation)
        {
            Description = description;
            TicketLocation = ticketLocation;
        }



        public static TicketDTOHandler NoFreeParkingLot()
        {
            return new TicketDTOHandler(description: "No available parking lots", ticketLocation: null);
        }

        public static TicketDTOHandler NotFound()
        {
            return new TicketDTOHandler(description: "Ticket not found", ticketLocation: null);
        }

        public static TicketDTOHandler Created(string ticketLocation)
        {
            return new TicketDTOHandler(description: null, ticketLocation: ticketLocation);
        }

        public static TicketDTOHandler AlreadyAvailable()
        {
            return new TicketDTOHandler(description: "Parking lot is already available", ticketLocation: null);
        }


    }

}
