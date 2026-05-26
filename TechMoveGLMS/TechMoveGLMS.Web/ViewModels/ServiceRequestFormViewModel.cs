using System.ComponentModel.DataAnnotations;
using TechMoveGLMS.Web.Models.Enums;

namespace TechMoveGLMS.Web.ViewModels;

public class ServiceRequestFormViewModel
{
    public int Id { get; set; }

    [Required, Display(Name = "Contract")]
    public int ContractId { get; set; }

    [Required, StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required, Range(0.01, 100000000), Display(Name = "Cost USD")]
    public decimal CostUsd { get; set; }

    [Required]
    public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Pending;
}
