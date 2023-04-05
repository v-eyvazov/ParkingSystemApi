using System.Net.Http.Headers;

namespace ParkingSystemAPI.Services.Impl
{
    public class QRReaderClientService : IQRReaderClientService
    {
        public async Task<string> DecryptQR(byte[] pdfByteArray)
        {
            var data = new ByteArrayContent(pdfByteArray);
            data.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            var uri = "http://127.0.0.1:5000/value";

            using var client = new HttpClient();

            var response = await client.PostAsync(uri, data);

            var result = await response.Content.ReadAsStringAsync();

            return result;

        }
    }
}
