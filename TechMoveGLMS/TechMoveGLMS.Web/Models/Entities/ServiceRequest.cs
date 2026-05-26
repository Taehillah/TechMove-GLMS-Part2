using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechMoveGLMS.Web.Models.Enums;

namespace TechMoveGLMS.Web.Models.Entities;

public class ServiceRequest
{
    public int Id { get; set; }

    [Required, Display(Name = "Contract")]
    public int ContractId { get; set; }

    public Contract? Contract { get; set; }

    [Required, StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required, Range(0.01, 100000000), Column(TypeName = "decimal(18,2)"), Display(Name = "Cost USD")]
    public decimal CostUsd { get; set; }

    [Required, Column(TypeName = "decimal(18,4)"), Display(Name = "USD to ZAR Rate")]
    public decimal ExchangeRateUsdToZar { get; set; }

    [Required, Column(TypeName = "decimal(18,2)"), Display(Name = "Cost ZAR")]
    public decimal CostZar { get; set; }

    [Required]
    public ServiceRequestStatus Status { get; set; }

    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }
}
