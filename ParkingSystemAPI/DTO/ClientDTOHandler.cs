using System.Text.Json.Serialization;

namespace ParkingSystemAPI.DTO
{
    public class ClientDTOHandler
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        private ClientDTOHandler(string? description)
        {
            Description = description;
        }

        public static ClientDTOHandler Timeout()
        {
            return new ClientDTOHandler("Web service timeout");
        }

        public static ClientDTOHandler InvalidUri()
        {
            return new ClientDTOHandler("Invalid uri");
        }
    }
}
