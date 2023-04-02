namespace ParkingSystemAPI.Services.Impl
{


    public class FilePathUtilService : IFilePathUtilService
    {
        private readonly IConfiguration configuration;

        //for unit tests
        private readonly Func<string, DirectoryInfo> createDirectory;

        public FilePathUtilService(IConfiguration configuration, Func<string, DirectoryInfo>? createDirectory = null)
        {
            this.configuration = configuration;
            this.createDirectory = createDirectory ?? Directory.CreateDirectory;
        }

        public string GetFilePath(string filename)
        {

            string? directory = configuration["TicketDirectory"];
            directory ??= "QRfiles";

            string filePath = Path.Combine(directory, filename + ".pdf");
            createDirectory(directory);
            return filePath;

        }
    }
}
