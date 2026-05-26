namespace TechMoveGLMS.Web.Services;

public interface IContractFileService
{
    Task<string> SaveSignedAgreementAsync(IFormFile file);
}
