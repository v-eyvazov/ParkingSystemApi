using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace ParkingSystemAPI.Services.Impl
{
    public class PdfSharpCorePDFGeneratorService : IPDFGeneratorService<PdfSharpCorePDFGeneratorService>
    {

        private readonly IFilePathUtilService _filePathUtilService;


        public PdfSharpCorePDFGeneratorService(IFilePathUtilService filePathUtilService)
        {
            _filePathUtilService = filePathUtilService;
        }

        public string GeneratePDF(byte[] content, string filename)
        {

            PdfDocument document = new();
            document.Info.Title = filename;
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            using var ms = new MemoryStream(content);
            XImage image = XImage.FromStream(() => ms);
            XPoint point = new(127, 255);
            gfx.DrawImage(image, point);

            string filePath = _filePathUtilService.GetFilePath(filename);

            document.Save(filePath);
            return filePath;
        }
    }
}
