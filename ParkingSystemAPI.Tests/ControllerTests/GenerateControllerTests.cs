using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingSystemAPI.Controllers;
using ParkingSystemAPI.DTO;
using ParkingSystemAPI.Services;
using ParkingSystemAPI.Services.Impl;

namespace ParkingSystemAPI.Tests.ControllerTests
{
    public class GenerateControllerTests
    {

        private readonly ILogger<GenerateController> _logger;
        private readonly ITicketService _ticketService;
        private readonly IQRGeneratorService _qrGenerator;
        private readonly IPDFGeneratorService<PdfSharpCorePDFGeneratorService> _pdfGenerator;
        private readonly GenerateController _generateController;

        public GenerateControllerTests()
        {
            //Dependencies
            _logger = A.Fake<ILogger<GenerateController>>();
            _ticketService = A.Fake<ITicketService>();
            _qrGenerator = A.Fake<IQRGeneratorService>();
            _pdfGenerator = A.Fake<IPDFGeneratorService<PdfSharpCorePDFGeneratorService>>();

            //SUTr
            _generateController = new GenerateController(_logger, _ticketService, _qrGenerator, _pdfGenerator);
        }

        [Fact]
        public void GenerateController_Reserve_ReturnsSuccess()
        {
            //Arrange
            var activityDTO = A.Fake<ActivityDTO>();
            A.CallTo(() => _ticketService.ReserveParkingLot()).Returns(activityDTO);

            var qr = new byte[] { 9, 10 };
            A.CallTo(() => _qrGenerator.GenerateQR(activityDTO.TicketNumber)).Returns(qr);

            var filePath = "test";
            A.CallTo(() => _pdfGenerator.GeneratePDF(qr, activityDTO.Id.ToString())).Returns(filePath);

            //Act
            var result = _generateController.Reserve();

            //Assert
            result.Should().BeOfType<ActionResult<TicketDTOHandler>>();
        }
    }
}
