using System.Text.Json.Serialization;

namespace ParkingSystemAPI.DTO
{
    public class CheckDTOHandler
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        private CheckDTOHandler(string? description)
        {
            Description = description;
        }

        public static CheckDTOHandler Success()
        {
            return new CheckDTOHandler("success");
        }
    }
}


// Temporary