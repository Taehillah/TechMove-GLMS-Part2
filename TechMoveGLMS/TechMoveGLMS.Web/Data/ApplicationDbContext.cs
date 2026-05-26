using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.Web.Models.Entities;
using TechMoveGLMS.Web.Models.Enums;

namespace TechMoveGLMS.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();

    public DbSet<Contract> Contracts => Set<Contract>();

    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .HasMany(client => client.Contracts)
            .WithOne(contract => contract.Client)
            .HasForeignKey(contract => contract.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contract>()
            .HasMany(contract => contract.ServiceRequests)
            .WithOne(request => request.Contract)
            .HasForeignKey(request => request.ContractId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, Name = "Northwind Retail Group", ContactDetails = "operations@northwind.example | +27 11 555 0101", Region = "Gauteng" },
            new Client { Id = 2, Name = "Cape Export Traders", ContactDetails = "logistics@capeexport.example | +27 21 555 0188", Region = "Western Cape" });

        modelBuilder.Entity<Contract>().HasData(
            new Contract { Id = 1, ClientId = 1, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), Status = ContractStatus.Active, ServiceLevel = "Priority Freight", SignedAgreementFileName = null, SignedAgreementPath = null },
            new Contract { Id = 2, ClientId = 1, StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 12, 31), Status = ContractStatus.Expired, ServiceLevel = "Warehousing", SignedAgreementFileName = null, SignedAgreementPath = null },
            new Contract { Id = 3, ClientId = 2, StartDate = new DateTime(2026, 3, 1), EndDate = new DateTime(2027, 2, 28), Status = ContractStatus.OnHold, ServiceLevel = "Customs Clearance", SignedAgreementFileName = null, SignedAgreementPath = null });

        modelBuilder.Entity<ServiceRequest>().HasData(
            new ServiceRequest { Id = 1, ContractId = 1, Description = "Urgent Johannesburg to Durban freight movement.", CostUsd = 1000m, ExchangeRateUsdToZar = 18.50m, CostZar = 18500m, Status = ServiceRequestStatus.Approved, CreatedAt = new DateTime(2026, 4, 1, 8, 30, 0, DateTimeKind.Utc) },
            new ServiceRequest { Id = 2, ContractId = 1, Description = "Temporary bonded warehouse handling for imported stock.", CostUsd = 750m, ExchangeRateUsdToZar = 18.50m, CostZar = 13875m, Status = ServiceRequestStatus.Pending, CreatedAt = new DateTime(2026, 4, 15, 10, 0, 0, DateTimeKind.Utc) });
    }
}
