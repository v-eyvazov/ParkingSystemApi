using System.Text.Json.Serialization;

namespace ParkingSystemAPI.DTO
{
    public class FileErrorDTOHandler
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        private FileErrorDTOHandler(string? description)
        {
            Description = description;
        }

        public static FileErrorDTOHandler InvalidRequest()
        {
            return new FileErrorDTOHandler(description: "Invalid request");
        }

        public static FileErrorDTOHandler NetworkProblem()
        {
            return new FileErrorDTOHandler(description: "Ooops, something went wronh :/");
        }

        public static FileErrorDTOHandler NotFound()
        {
            return new FileErrorDTOHandler(description: "No such file");
        }

    }
}
