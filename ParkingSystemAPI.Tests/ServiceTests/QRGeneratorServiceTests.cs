using FluentAssertions;
using ParkingSystemAPI.Services;
using ParkingSystemAPI.Services.Impl;

namespace ParkingSystemAPI.Tests.ServiceTests
{
    public class QRGeneratorServiceTests
    {

        private readonly IQRGeneratorService _qRGeneratorService;

        public QRGeneratorServiceTests()
        {
            //Dependencies


            //SUT
            _qRGeneratorService = new QRGeneratorService();
        }

        [Fact]
        public void QRGeneratorService_GenerateQR_ReturnsByteArray()
        {
            //Arrange
            string data = "test";

            //Act
            var qr = _qRGeneratorService.GenerateQR(data);

            //Assert
            qr.Should().BeOfType<byte[]>();
        }
    }
}
