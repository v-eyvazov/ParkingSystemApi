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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TicketDTOHandler))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(TicketDTOHandler))]
        public ActionResult<TicketDTOHandler> Reserve()
        {
            ActivityDTO activityDTO;

            try
            {
                activityDTO = _ticketService.ReserveParkingLot();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, TicketDTOHandler.NoFreeParkingLot());
            }

            byte[] qr = _qrGenerator.GenerateQR(activityDTO.TicketNumber);

            string filePath = _pdfGenerator.GeneratePDF(qr, activityDTO.Id.ToString());

            return StatusCode(StatusCodes.Status201Created, TicketDTOHandler.Created(filePath));

        }

        [HttpPost]
        [Route("download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FileErrorDTOHandler))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(FileErrorDTOHandler))]
        public IActionResult DownloadTicket([FromBody] FilePathDTO filePathDTO)
        {

            if (filePathDTO.FilePath == null || filePathDTO.FilePath.Length == 0)
            {
                return BadRequest(FileErrorDTOHandler.InvalidRequest());
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
                return NotFound(FileErrorDTOHandler.NotFound());
            }
        }

    }
}
