using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace ParkingSystemAPI.Services.Impl
{
    public class SyncfusionPDFGeneratorService : IPDFGeneratorService<SyncfusionPDFGeneratorService>
    {

        private readonly IFilePathUtilService _filePathUtilService;


        public SyncfusionPDFGeneratorService(IFilePathUtilService filePathUtilService)
        {
            _filePathUtilService = filePathUtilService;
        }


        public string GeneratePDF(byte[] content, string filename)
        {
            PdfDocument document = new();
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            //Converts byte array into Bitmap to create PDF
            using (var byteArray = new MemoryStream(content))
            {
                PdfBitmap image = new(byteArray);
                graphics.DrawImage(image, 0, 0);
            }



            using MemoryStream ms = new();
            document.Save(ms);
            ms.Position = 0;
            document.Close(true);

            string filePath = _filePathUtilService.GetFilePath(filename);

            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, (int)ms.Length);
                file.Write(bytes, 0, bytes.Length);
            }
            return filePath;

        }
    }
}
