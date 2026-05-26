namespace TechMoveGLMS.Web.Services;

public class ContractFileService(IWebHostEnvironment environment, IFileValidationService fileValidationService) : IContractFileService
{
    public async Task<string> SaveSignedAgreementAsync(IFormFile file)
    {
        fileValidationService.ValidateContractPdf(file);

        var uploadsRoot = Path.Combine(environment.WebRootPath, "uploads", "contracts");
        Directory.CreateDirectory(uploadsRoot);

        var safeOriginalName = Path.GetFileName(file.FileName);
        var storedFileName = $"{Guid.NewGuid():N}_{safeOriginalName}";
        var fullPath = Path.Combine(uploadsRoot, storedFileName);

        await using var stream = File.Create(fullPath);
        await file.CopyToAsync(stream);

        return $"/uploads/contracts/{storedFileName}";
    }
}
