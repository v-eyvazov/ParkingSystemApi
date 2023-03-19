using System.Text.Json.Serialization;

namespace ParkingSystemAPI.DTO
{
    public class TicketDTO
    {

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Desctiption { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TicketLocation { get; set; }



        private TicketDTO(string? description, string? ticketLocation)
        {
            Desctiption = description;
            TicketLocation = ticketLocation;
        }



        public static TicketDTO NotFound(string description)
        {
            return new TicketDTO(description: description, ticketLocation: null);
        }

        public static TicketDTO Created(string ticketLocation)
        {
            return new TicketDTO(description: null, ticketLocation: ticketLocation);
        }


    }

}
