using Microsoft.AspNetCore.Mvc;
using ParkingSystemAPI.DTO;
using ParkingSystemAPI.Services;

namespace ParkingSystemAPI.Controllers
{

    [ApiController]
    [Route("api")]
    public class OperationsController : Controller
    {

        private readonly ITicketService _ticketService;
        private readonly ILogger<OperationsController> _logger;
        private readonly IQRReaderClient _qrReaderClient;

        public OperationsController(ITicketService ticketService, ILogger<OperationsController> logger, IQRReaderClient qRReaderClient)
        {
            _ticketService = ticketService;
            _logger = logger;
            _qrReaderClient = qRReaderClient;

        }

        [HttpPost]
        [Route("checkout")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(FileErrorDTOHandler))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(TicketDTOHandler))]
        public async Task<ActionResult> Checkout([FromBody] FilePathDTO filePathDTO)
        {

            byte[] pdfByteArray;

            // <--------------------------  Get the file byte array  ----------------------------->
            try
            {
                pdfByteArray = System.IO.File.ReadAllBytes(filePathDTO.FilePath);
            }
            catch (Exception ex) when (
                ex is ArgumentException
                || ex is ArgumentNullException
                || ex is PathTooLongException
                || ex is DirectoryNotFoundException
                || ex is FileNotFoundException
                || ex is NotSupportedException
                )
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, FileErrorDTOHandler.InvalidRequest());
            }
            catch (Exception ex) when (
                ex is IOException
                )
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, FileErrorDTOHandler.NetworkProblem());
            }



            // <--------------------------------  Decrypt QR  -------------------------------------->
            // needs try catch clause
            string result = await _qrReaderClient.DecryptQR(pdfByteArray);



            // <-------------------------------  Checkout in DB  ------------------------------------>
            ActivityDTO activityDTO;
            try
            {
                activityDTO = _ticketService.CheckoutParkingLot(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, TicketDTOHandler.NotFound());
            }

            return Ok(CheckDTOHandler.Success());

        }

    }
}
