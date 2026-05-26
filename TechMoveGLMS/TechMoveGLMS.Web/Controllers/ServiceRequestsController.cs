using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Web.Data;
using TechMoveGLMS.Web.Models.Entities;
using TechMoveGLMS.Web.Services;
using TechMoveGLMS.Web.ViewModels;

namespace TechMoveGLMS.Web.Controllers;

public class ServiceRequestsController(
    ApplicationDbContext context,
    ICurrencyService currencyService,
    IServiceRequestRulesService rulesService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var requests = await context.ServiceRequests
            .Include(request => request.Contract)
            .ThenInclude(contract => contract!.Client)
            .OrderByDescending(request => request.CreatedAt)
            .ToListAsync();

        return View(requests);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var request = await context.ServiceRequests
            .Include(serviceRequest => serviceRequest.Contract)
            .ThenInclude(contract => contract!.Client)
            .FirstOrDefaultAsync(serviceRequest => serviceRequest.Id == id);

        return request is null ? NotFound() : View(request);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateContractsAsync();
        return View(new ServiceRequestFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ServiceRequestFormViewModel model)
    {
        var contract = await context.Contracts
            .Include(existingContract => existingContract.Client)
            .FirstOrDefaultAsync(existingContract => existingContract.Id == model.ContractId);

        if (contract is null)
        {
            ModelState.AddModelError(nameof(model.ContractId), "Please select a valid contract.");
        }
        else
        {
            try
            {
                rulesService.ValidateContractAllowsServiceRequest(contract);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(model.ContractId), ex.Message);
            }
        }

        if (!ModelState.IsValid)
        {
            await PopulateContractsAsync(model.ContractId);
            return View(model);
        }

        decimal exchangeRate;
        try
        {
            exchangeRate = await currencyService.GetUsdToZarRateAsync();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateContractsAsync(model.ContractId);
            return View(model);
        }

        var request = new ServiceRequest
        {
            ContractId = model.ContractId,
            Description = model.Description,
            CostUsd = model.CostUsd,
            ExchangeRateUsdToZar = exchangeRate,
            CostZar = currencyService.ConvertUsdToZar(model.CostUsd, exchangeRate),
            Status = model.Status,
            CreatedAt = DateTime.UtcNow
        };

        context.Add(request);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var request = await context.ServiceRequests.FindAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        await PopulateContractsAsync(request.ContractId);
        return View(new ServiceRequestFormViewModel
        {
            Id = request.Id,
            ContractId = request.ContractId,
            Description = request.Description,
            CostUsd = request.CostUsd,
            Status = request.Status
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ServiceRequestFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var request = await context.ServiceRequests.FindAsync(id);
        if (request is null)
        {
            return NotFound();
        }

        var contract = await context.Contracts.FindAsync(model.ContractId);
        if (contract is null)
        {
            ModelState.AddModelError(nameof(model.ContractId), "Please select a valid contract.");
        }
        else
        {
            try
            {
                rulesService.ValidateContractAllowsServiceRequest(contract);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(model.ContractId), ex.Message);
            }
        }

        if (!ModelState.IsValid)
        {
            await PopulateContractsAsync(model.ContractId);
            return View(model);
        }

        decimal exchangeRate;
        try
        {
            exchangeRate = await currencyService.GetUsdToZarRateAsync();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateContractsAsync(model.ContractId);
            return View(model);
        }

        request.ContractId = model.ContractId;
        request.Description = model.Description;
        request.CostUsd = model.CostUsd;
        request.ExchangeRateUsdToZar = exchangeRate;
        request.CostZar = currencyService.ConvertUsdToZar(model.CostUsd, exchangeRate);
        request.Status = model.Status;

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var request = await context.ServiceRequests
            .Include(serviceRequest => serviceRequest.Contract)
            .ThenInclude(contract => contract!.Client)
            .FirstOrDefaultAsync(serviceRequest => serviceRequest.Id == id);

        return request is null ? NotFound() : View(request);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var request = await context.ServiceRequests.FindAsync(id);
        if (request is not null)
        {
            context.ServiceRequests.Remove(request);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateContractsAsync(int? selectedContractId = null)
    {
        var contracts = await context.Contracts
            .Include(contract => contract.Client)
            .OrderBy(contract => contract.Client!.Name)
            .ThenBy(contract => contract.ServiceLevel)
            .ToListAsync();

        ViewBag.ContractId = new SelectList(
            contracts.Select(contract => new
            {
                contract.Id,
                Label = $"{contract.Client?.Name} - {contract.ServiceLevel} ({contract.Status})"
            }),
            "Id",
            "Label",
            selectedContractId);
    }
}
