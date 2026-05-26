using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Web.Data;
using TechMoveGLMS.Web.Models.Entities;
using TechMoveGLMS.Web.Services;
using TechMoveGLMS.Web.ViewModels;

namespace TechMoveGLMS.Web.Controllers;

public class ContractsController(ApplicationDbContext context, IContractFileService contractFileService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var contracts = await context.Contracts
            .Include(contract => contract.Client)
            .OrderByDescending(contract => contract.StartDate)
            .ToListAsync();

        return View(contracts);
    }

    public async Task<IActionResult> Search(ContractFilterViewModel filters)
    {
        IQueryable<Contract> query = context.Contracts
            .Include(contract => contract.Client);

        if (filters.Status.HasValue)
        {
            query = query.Where(contract => contract.Status == filters.Status.Value);
        }

        if (filters.StartDate.HasValue)
        {
            query = query.Where(contract => contract.StartDate >= filters.StartDate.Value);
        }

        if (filters.EndDate.HasValue)
        {
            query = query.Where(contract => contract.EndDate <= filters.EndDate.Value);
        }

        filters.Results = await query
            .OrderByDescending(contract => contract.StartDate)
            .ToListAsync();

        return View(filters);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var contract = await context.Contracts
            .Include(contract => contract.Client)
            .Include(contract => contract.ServiceRequests)
            .FirstOrDefaultAsync(contract => contract.Id == id);

        return contract is null ? NotFound() : View(contract);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateClientsAsync();
        return View(new ContractFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContractFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateClientsAsync(model.ClientId);
            return View(model);
        }

        var contract = new Contract
        {
            ClientId = model.ClientId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Status = model.Status,
            ServiceLevel = model.ServiceLevel
        };

        if (model.SignedAgreement is not null)
        {
            try
            {
                contract.SignedAgreementPath = await contractFileService.SaveSignedAgreementAsync(model.SignedAgreement);
                contract.SignedAgreementFileName = Path.GetFileName(model.SignedAgreement.FileName);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(model.SignedAgreement), ex.Message);
                await PopulateClientsAsync(model.ClientId);
                return View(model);
            }
        }

        context.Add(contract);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var contract = await context.Contracts.FindAsync(id);
        if (contract is null)
        {
            return NotFound();
        }

        await PopulateClientsAsync(contract.ClientId);
        return View(new ContractFormViewModel
        {
            Id = contract.Id,
            ClientId = contract.ClientId,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Status = contract.Status,
            ServiceLevel = contract.ServiceLevel,
            ExistingSignedAgreementFileName = contract.SignedAgreementFileName
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContractFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var contract = await context.Contracts.FindAsync(id);
        if (contract is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateClientsAsync(model.ClientId);
            return View(model);
        }

        contract.ClientId = model.ClientId;
        contract.StartDate = model.StartDate;
        contract.EndDate = model.EndDate;
        contract.Status = model.Status;
        contract.ServiceLevel = model.ServiceLevel;

        if (model.SignedAgreement is not null)
        {
            try
            {
                contract.SignedAgreementPath = await contractFileService.SaveSignedAgreementAsync(model.SignedAgreement);
                contract.SignedAgreementFileName = Path.GetFileName(model.SignedAgreement.FileName);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(model.SignedAgreement), ex.Message);
                model.ExistingSignedAgreementFileName = contract.SignedAgreementFileName;
                await PopulateClientsAsync(model.ClientId);
                return View(model);
            }
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var contract = await context.Contracts
            .Include(contract => contract.Client)
            .FirstOrDefaultAsync(contract => contract.Id == id);

        return contract is null ? NotFound() : View(contract);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contract = await context.Contracts.FindAsync(id);
        if (contract is not null)
        {
            context.Contracts.Remove(contract);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateClientsAsync(int? selectedClientId = null)
    {
        var clients = await context.Clients
            .OrderBy(client => client.Name)
            .ToListAsync();

        ViewBag.ClientId = new SelectList(clients, "Id", "Name", selectedClientId);
    }
}
