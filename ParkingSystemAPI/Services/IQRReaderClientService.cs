namespace ParkingSystemAPI.Services
{
    public interface IQRReaderClientService
    {
        Task<string> DecryptQR(byte[] pdfByteArray);
    }
}
