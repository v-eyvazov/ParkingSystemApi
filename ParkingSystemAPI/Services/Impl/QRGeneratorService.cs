using QRCoder;

namespace ParkingSystemAPI.Services.Impl
{
    public class QRGeneratorService : IQRGeneratorService
    {
        public byte[] GenerateQR(string data)
        {
            QRCodeGenerator qRCodeGenerator = new();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode pngByteQRCode = new(qRCodeData);
            byte[] qrCodeAsBitmapByteArr = pngByteQRCode.GetGraphic(10);
            return qrCodeAsBitmapByteArr;
        }
    }
}
