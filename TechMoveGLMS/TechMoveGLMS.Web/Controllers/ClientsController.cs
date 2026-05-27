using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Web.Data;
using TechMoveGLMS.Web.Models.Entities;

namespace TechMoveGLMS.Web.Controllers;

public class ClientsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var clients = await context.Clients
            .OrderBy(client => client.Name)
            .ToListAsync();

        return View(clients);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var client = await context.Clients
            .Include(client => client.Contracts)
            .FirstOrDefaultAsync(client => client.Id == id);

        return client is null ? NotFound() : View(client);
    }

    public IActionResult Create()
    {
        return View(new Client());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Client client)
    {
        if (!ModelState.IsValid)
        {
            return View(client);
        }

        context.Add(client);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var client = await context.Clients.FindAsync(id);
        return client is null ? NotFound() : View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Client client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(client);
        }

        context.Update(client);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var client = await context.Clients
            .FirstOrDefaultAsync(client => client.Id == id);

        return client is null ? NotFound() : View(client);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var client = await context.Clients
            .Include(existingClient => existingClient.Contracts)
            .FirstOrDefaultAsync(existingClient => existingClient.Id == id);

        if (client is not null)
        {
            if (client.Contracts.Count > 0)
            {
                TempData["StatusMessage"] = "Client cannot be deleted while contracts are linked to it.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            context.Clients.Remove(client);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
