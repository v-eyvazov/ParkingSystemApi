using System.Text.Json.Serialization;

namespace ParkingSystemAPI.DTO
{
    public class ClientDTOHandler
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Desctiption { get; set; }

        private ClientDTOHandler(string? description)
        {
            Desctiption = description;
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
