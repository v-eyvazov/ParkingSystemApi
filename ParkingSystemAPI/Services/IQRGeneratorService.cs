namespace ParkingSystemAPI.Services
{
    public interface IQRGeneratorService
    {
        public byte[] GenerateQR(string data);
    }
}
