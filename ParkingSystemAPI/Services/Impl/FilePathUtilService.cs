namespace ParkingSystemAPI.Services.Impl
{

    // for unit tests
    public delegate DirectoryInfo DirectoryDel(string directory);

    public class FilePathUtilService : IFilePathUtilService
    {
        private readonly IConfiguration configuration;
        private readonly DirectoryDel createDirectory;

        public FilePathUtilService(IConfiguration configuration, DirectoryDel? createDirectory = null)
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
