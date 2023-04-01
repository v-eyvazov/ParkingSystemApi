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
    private readonly DirectoryDel _createDirectory;

    public FilePathUtilServiceTests()
    {

        //Dependencies
        _configuration = A.Fake<IConfiguration>();
        _createDirectory = A.Fake<DirectoryDel>();

        //SUT
        _filePathUtilService = new FilePathUtilService(_configuration, _createDirectory);
    }

    [Theory]
    [InlineData("Test")]
    public void FilePathUtilService_GetFilePath_ReturnString(string filename)
    {
        //Arrange


        //Act
        string filePath = _filePathUtilService.GetFilePath(filename);

        //Assert
        filePath.Should().NotBeNullOrWhiteSpace();
        filePath.Should().BeOfType<string>();
        filePath.Should().Contain(".pdf");
    }
}
