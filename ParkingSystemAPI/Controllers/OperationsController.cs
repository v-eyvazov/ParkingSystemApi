using Microsoft.AspNetCore.Mvc;
using ParkingSystemAPI.DTO;
using ParkingSystemAPI.Exceptions;
using ParkingSystemAPI.Services;

namespace ParkingSystemAPI.Controllers
{

    [ApiController]
    [Route("api")]
    public class OperationsController : Controller
    {

        private readonly ITicketService _ticketService;
        private readonly ILogger<OperationsController> _logger;
        private readonly IQRReaderClientService _qrReaderClientService;

        public OperationsController(ITicketService ticketService, ILogger<OperationsController> logger, IQRReaderClientService qRReaderClientService)
        {
            _ticketService = ticketService;
            _logger = logger;
            _qrReaderClientService = qRReaderClientService;

        }

        [HttpPost]
        [Route("checkout")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(FileErrorDTOHandler))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(TicketDTOHandler))]
        [ProducesResponseType(StatusCodes.Status208AlreadyReported, Type = typeof(TicketDTOHandler))]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout, Type = typeof(ClientDTOHandler))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ClientDTOHandler))]
        public async Task<ActionResult> Checkout(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, FileErrorDTOHandler.InvalidRequest());
            }

            #region "Get file byte array"
            byte[] pdfByteArray;
            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                pdfByteArray = ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, FileErrorDTOHandler.NetworkProblem());
            }
            #endregion


            #region "Decrypt QR"
            string result;
            try
            {
                result = await _qrReaderClientService.DecryptQR(pdfByteArray);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status408RequestTimeout, ClientDTOHandler.Timeout());
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is UriFormatException)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, ClientDTOHandler.InvalidUri());
            }
            #endregion


            #region "Checkout in DB"
            try
            {
                _ticketService.CheckoutParkingLot(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, TicketDTOHandler.NotFound());
            }
            catch (AlreadyAvailableException al)
            {
                _logger.LogError(al.Message);
                return StatusCode(StatusCodes.Status208AlreadyReported, TicketDTOHandler.AlreadyAvailable());
            }
            #endregion

            return Ok(CheckDTOHandler.Success());

        }

        [HttpPost]
        [Route("cancel")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(FileErrorDTOHandler))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(TicketDTOHandler))]
        [ProducesResponseType(StatusCodes.Status208AlreadyReported, Type = typeof(TicketDTOHandler))]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout, Type = typeof(ClientDTOHandler))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ClientDTOHandler))]
        public async Task<ActionResult> Cancel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, FileErrorDTOHandler.InvalidRequest());
            }

            #region "Get file byte array"
            byte[] pdfByteArray;
            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                pdfByteArray = ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, FileErrorDTOHandler.NetworkProblem());
            }
            #endregion

            #region "Decrypt QR"
            string result;
            try
            {
                result = await _qrReaderClientService.DecryptQR(pdfByteArray);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status408RequestTimeout, ClientDTOHandler.Timeout());
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is UriFormatException)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, ClientDTOHandler.InvalidUri());
            }
            #endregion

            #region "Cancel in DB"
            try
            {
                _ticketService.CancelParkingLot(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, TicketDTOHandler.NotFound());
            }
            catch (AlreadyAvailableException al)
            {
                _logger.LogError(al.Message);
                return StatusCode(StatusCodes.Status208AlreadyReported, TicketDTOHandler.AlreadyAvailable());
            }
            #endregion

            return Ok(CheckDTOHandler.Success());

        }

    }
}
