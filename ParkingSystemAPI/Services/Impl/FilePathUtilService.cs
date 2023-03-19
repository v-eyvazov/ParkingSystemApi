namespace ParkingSystemAPI.Services.Impl
{
    public class FilePathUtilService : IFilePathUtilService
    {
        private readonly IConfiguration configuration;

        public FilePathUtilService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetFilePath(string filename)
        {

            string? directory = configuration["TicketDirectory"];
            directory ??= "Null";

            string filePath = Path.Combine(directory, filename + ".pdf");
            Directory.CreateDirectory(directory);

            return filePath;

        }
    }
}
