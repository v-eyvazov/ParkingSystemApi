using System.Text.Json.Serialization;

namespace ParkingSystemAPI.DTO
{
    public class CheckDTOHandler
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Desctiption { get; set; }

        private CheckDTOHandler(string? description)
        {
            Desctiption = description;
        }

        public static CheckDTOHandler Success()
        {
            return new CheckDTOHandler("success");
        }
    }
}


// Temporary