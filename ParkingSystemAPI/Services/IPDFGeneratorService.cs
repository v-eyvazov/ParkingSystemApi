namespace ParkingSystemAPI.Services
{
    public interface IPDFGeneratorService<T> where T : class
    {
        public string GeneratePDF(byte[] content, string filename);
    }
}
