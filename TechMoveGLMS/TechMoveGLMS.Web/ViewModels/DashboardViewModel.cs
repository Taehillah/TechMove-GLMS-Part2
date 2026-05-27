using TechMoveGLMS.Web.Models.Entities;

namespace TechMoveGLMS.Web.ViewModels;

public class DashboardViewModel
{
    public int TotalClients { get; set; }

    public int TotalContracts { get; set; }

    public int TotalServiceRequests { get; set; }

    public int ActiveContracts { get; set; }

    public int PendingRequests { get; set; }

    public int CompletedRequests { get; set; }

    public decimal TotalFreightValueZar { get; set; }

    public IReadOnlyList<ServiceRequest> RecentRequests { get; set; } = [];
}
