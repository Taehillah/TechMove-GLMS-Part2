using TechMoveGLMS.Web.Models.Entities;
using TechMoveGLMS.Web.Models.Enums;

namespace TechMoveGLMS.Web.ViewModels;

public class ContractFilterViewModel
{
    public ContractStatus? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public IReadOnlyList<Contract> Results { get; set; } = [];
}
