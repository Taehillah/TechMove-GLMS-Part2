using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechMoveGLMS.Web.Data;

#nullable disable

namespace TechMoveGLMS.Web.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.11")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("TechMoveGLMS.Web.Models.Entities.Client", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<string>("ContactDetails").IsRequired().HasMaxLength(250).HasColumnType("nvarchar(250)");
            b.Property<string>("Name").IsRequired().HasMaxLength(120).HasColumnType("nvarchar(120)");
            b.Property<string>("Region").IsRequired().HasMaxLength(80).HasColumnType("nvarchar(80)");
            b.HasKey("Id");
            b.ToTable("Clients");
            b.HasData(
                new { Id = 1, ContactDetails = "operations@northwind.example | +27 11 555 0101", Name = "Northwind Retail Group", Region = "Gauteng" },
                new { Id = 2, ContactDetails = "logistics@capeexport.example | +27 21 555 0188", Name = "Cape Export Traders", Region = "Western Cape" });
        });

        modelBuilder.Entity("TechMoveGLMS.Web.Models.Entities.Contract", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<int>("ClientId").HasColumnType("int");
            b.Property<DateTime>("EndDate").HasColumnType("datetime2");
            b.Property<string>("ServiceLevel").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");
            b.Property<string>("SignedAgreementFileName").HasColumnType("nvarchar(max)");
            b.Property<string>("SignedAgreementPath").HasColumnType("nvarchar(max)");
            b.Property<DateTime>("StartDate").HasColumnType("datetime2");
            b.Property<int>("Status").HasColumnType("int");
            b.HasKey("Id");
            b.HasIndex("ClientId");
            b.ToTable("Contracts");
            b.HasData(
                new { Id = 1, ClientId = 1, EndDate = new DateTime(2026, 12, 31), ServiceLevel = "Priority Freight", StartDate = new DateTime(2026, 1, 1), Status = 1 },
                new { Id = 2, ClientId = 1, EndDate = new DateTime(2025, 12, 31), ServiceLevel = "Warehousing", StartDate = new DateTime(2025, 1, 1), Status = 2 },
                new { Id = 3, ClientId = 2, EndDate = new DateTime(2027, 2, 28), ServiceLevel = "Customs Clearance", StartDate = new DateTime(2026, 3, 1), Status = 3 });
        });

        modelBuilder.Entity("TechMoveGLMS.Web.Models.Entities.ServiceRequest", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<int>("ContractId").HasColumnType("int");
            b.Property<decimal>("CostUsd").HasColumnType("decimal(18,2)");
            b.Property<decimal>("CostZar").HasColumnType("decimal(18,2)");
            b.Property<DateTime>("CreatedAt").HasColumnType("datetime2");
            b.Property<string>("Description").IsRequired().HasMaxLength(500).HasColumnType("nvarchar(500)");
            b.Property<decimal>("ExchangeRateUsdToZar").HasColumnType("decimal(18,4)");
            b.Property<int>("Status").HasColumnType("int");
            b.HasKey("Id");
            b.HasIndex("ContractId");
            b.ToTable("ServiceRequests");
            b.HasData(
                new { Id = 1, ContractId = 1, CostUsd = 1000m, CostZar = 18500m, CreatedAt = new DateTime(2026, 4, 1, 8, 30, 0, DateTimeKind.Utc), Description = "Urgent Johannesburg to Durban freight movement.", ExchangeRateUsdToZar = 18.50m, Status = 1 },
                new { Id = 2, ContractId = 1, CostUsd = 750m, CostZar = 13875m, CreatedAt = new DateTime(2026, 4, 15, 10, 0, 0, DateTimeKind.Utc), Description = "Temporary bonded warehouse handling for imported stock.", ExchangeRateUsdToZar = 18.50m, Status = 0 });
        });

        modelBuilder.Entity("TechMoveGLMS.Web.Models.Entities.Contract", b =>
        {
            b.HasOne("TechMoveGLMS.Web.Models.Entities.Client", "Client")
                .WithMany("Contracts")
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            b.Navigation("Client");
        });

        modelBuilder.Entity("TechMoveGLMS.Web.Models.Entities.ServiceRequest", b =>
        {
            b.HasOne("TechMoveGLMS.Web.Models.Entities.Contract", "Contract")
                .WithMany("ServiceRequests")
                .HasForeignKey("ContractId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            b.Navigation("Contract");
        });

        modelBuilder.Entity("TechMoveGLMS.Web.Models.Entities.Client", b => b.Navigation("Contracts"));
        modelBuilder.Entity("TechMoveGLMS.Web.Models.Entities.Contract", b => b.Navigation("ServiceRequests"));
#pragma warning restore 612, 618
    }
}
