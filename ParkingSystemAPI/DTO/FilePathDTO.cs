using System.ComponentModel.DataAnnotations;

namespace ParkingSystemAPI.DTO
{
    public class FilePathDTO
    {

        [Required]
        public string FilePath { get; set; }



        public FilePathDTO(string filePath)
        {
            FilePath = filePath;
        }

    }
}
