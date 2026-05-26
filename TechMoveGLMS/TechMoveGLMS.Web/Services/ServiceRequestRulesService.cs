using TechMoveGLMS.Web.Models.Entities;
using TechMoveGLMS.Web.Models.Enums;

namespace TechMoveGLMS.Web.Services;

public class ServiceRequestRulesService : IServiceRequestRulesService
{
    public void ValidateContractAllowsServiceRequest(Contract contract)
    {
        if (contract.Status == ContractStatus.Expired)
        {
            throw new InvalidOperationException("Service requests cannot be created for expired contracts.");
        }

        if (contract.Status == ContractStatus.OnHold)
        {
            throw new InvalidOperationException("Service requests cannot be created while a contract is on hold.");
        }
    }
}
