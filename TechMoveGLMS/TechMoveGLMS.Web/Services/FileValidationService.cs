namespace TechMoveGLMS.Web.Services;

public class FileValidationService : IFileValidationService
{
    private static readonly string[] AllowedContentTypes = ["application/pdf", "application/x-pdf"];

    public void ValidateContractPdf(IFormFile file)
    {
        if (file.Length == 0)
        {
            throw new InvalidOperationException("Please upload a non-empty PDF file.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only PDF signed agreements are allowed.");
        }

        if (!AllowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The uploaded signed agreement must be a PDF document.");
        }
    }
}
