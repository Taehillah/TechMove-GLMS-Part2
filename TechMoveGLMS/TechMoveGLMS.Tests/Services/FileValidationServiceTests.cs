using Microsoft.AspNetCore.Http;
using TechMoveGLMS.Web.Services;

namespace TechMoveGLMS.Tests.Services;

public class FileValidationServiceTests
{
    [Fact]
    public void ValidateContractPdf_AcceptsValidPdf()
    {
        // Arrange
        var service = new FileValidationService();
        var file = CreateFormFile("agreement.pdf", "application/pdf");

        // Act
        var exception = Record.Exception(() => service.ValidateContractPdf(file));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ValidateContractPdf_RejectsExeFiles()
    {
        // Arrange
        var service = new FileValidationService();
        var file = CreateFormFile("malware.exe", "application/x-msdownload");

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => service.ValidateContractPdf(file));

        // Assert
        Assert.Equal("Only PDF signed agreements are allowed.", exception.Message);
    }

    private static FormFile CreateFormFile(string fileName, string contentType)
    {
        var stream = new MemoryStream("%PDF-1.4"u8.ToArray());
        return new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }
}
