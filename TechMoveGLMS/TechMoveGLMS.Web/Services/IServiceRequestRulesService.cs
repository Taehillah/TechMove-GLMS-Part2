using TechMoveGLMS.Web.Models.Entities;

namespace TechMoveGLMS.Web.Services;

public interface IServiceRequestRulesService
{
    void ValidateContractAllowsServiceRequest(Contract contract);
}
