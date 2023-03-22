namespace ParkingSystemAPI.Services
{
    public interface IQRReaderClient
    {
        Task<string> DecryptQR(byte[] pdfByteArray);
    }
}
