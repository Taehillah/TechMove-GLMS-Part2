using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Web.Data;
using TechMoveGLMS.Web.Models;
using TechMoveGLMS.Web.Models.Enums;
using TechMoveGLMS.Web.ViewModels;

namespace TechMoveGLMS.Web.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var dashboard = new DashboardViewModel
        {
            TotalClients = await context.Clients.CountAsync(),
            TotalContracts = await context.Contracts.CountAsync(),
            TotalServiceRequests = await context.ServiceRequests.CountAsync(),
            ActiveContracts = await context.Contracts.CountAsync(contract => contract.Status == ContractStatus.Active),
            PendingRequests = await context.ServiceRequests.CountAsync(request => request.Status == ServiceRequestStatus.Pending),
            CompletedRequests = await context.ServiceRequests.CountAsync(request => request.Status == ServiceRequestStatus.Completed),
            TotalFreightValueZar = await context.ServiceRequests.SumAsync(request => (decimal?)request.CostZar) ?? 0m,
            RecentRequests = await context.ServiceRequests
                .Include(request => request.Contract)
                .ThenInclude(contract => contract!.Client)
                .OrderByDescending(request => request.CreatedAt)
                .Take(3)
                .ToListAsync()
        };

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
