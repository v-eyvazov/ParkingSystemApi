using Microsoft.AspNetCore.Mvc;
using ParkingSystemAPI.DTO;
using ParkingSystemAPI.Services;
using ParkingSystemAPI.Services.Impl;

namespace ParkingSystemAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class GenerateController : ControllerBase
    {

        private readonly ILogger<GenerateController> _logger;
        private readonly ITicketService _ticketService;
        private readonly IQRGeneratorService _qrGenerator;
        private readonly IPDFGeneratorService<PdfSharpCorePDFGeneratorService> _pdfGenerator;

        public GenerateController(ILogger<GenerateController> logger,
                                  ITicketService ticketService,
                                  IQRGeneratorService qRGenerator,
                                  IPDFGeneratorService<PdfSharpCorePDFGeneratorService> pdfGenerator)
        {
            _logger = logger;
            _ticketService = ticketService;
            _qrGenerator = qRGenerator;
            _pdfGenerator = pdfGenerator;
        }

        [HttpGet]
        [Route("reserve")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TicketDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(TicketDTO))]
        public ActionResult<TicketDTO> Reserve()
        {
            ActivityDTO activityDTO;

            try
            {
                activityDTO = _ticketService.Save();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, TicketDTO.NotFound("No available parking lots"));
            }

            byte[] qr = _qrGenerator.GenerateQR(activityDTO.TicketNumber);

            string filePath = _pdfGenerator.GeneratePDF(qr, activityDTO.Id.ToString());

            return StatusCode(StatusCodes.Status201Created, TicketDTO.Created(filePath));

        }

        [HttpPost]
        [Route("download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DownloadTicket([FromBody] FilePathDTO filePathDTO)
        {

            if (filePathDTO.FilePath == null || filePathDTO.FilePath.Length == 0)
            {
                return BadRequest(new { description = "Invalid request body" });
            }

            string mimeType = "application/pdf";

            try
            {
                byte[] pdf = System.IO.File.ReadAllBytes(filePathDTO.FilePath);
                var result = new FileContentResult(pdf, mimeType);
                result.FileDownloadName = Path.GetFileName(filePathDTO.FilePath);
                return result;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex.Message + "\nRequested source: " + filePathDTO.FilePath);
                return NotFound(new { description = "No such file" });
            }
        }

    }
}
