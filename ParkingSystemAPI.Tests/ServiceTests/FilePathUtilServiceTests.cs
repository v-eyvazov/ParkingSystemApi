using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using ParkingSystemAPI.Services;
using ParkingSystemAPI.Services.Impl;

namespace ParkingSystemAPI.Tests.ServiceTests;

public class FilePathUtilServiceTests
{

    private readonly IFilePathUtilService _filePathUtilService;
    private readonly IConfiguration _configuration;
    private readonly Func<string, DirectoryInfo> _createDirectory;

    public FilePathUtilServiceTests()
    {

        //Dependencies
        _configuration = A.Fake<IConfiguration>();
        _createDirectory = A.Fake<Func<string, DirectoryInfo>>();

        //SUT
        _filePathUtilService = new FilePathUtilService(_configuration, _createDirectory);
    }

    [Fact]
    public void FilePathUtilService_GetFilePath_ReturnsString()
    {
        //Arrange
        string filename = "test";

        //Act
        var filePath = _filePathUtilService.GetFilePath(filename);

        //Assert
        A.CallTo(() => _createDirectory(A<string>.Ignored)).MustHaveHappened();
        filePath.Should().NotBeNullOrWhiteSpace();
        filePath.Should().BeOfType<string>();
        filePath.Should().Contain(".pdf");
    }
}
