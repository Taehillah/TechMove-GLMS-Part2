using System.ComponentModel.DataAnnotations;
using TechMoveGLMS.Web.Models.Enums;

namespace TechMoveGLMS.Web.Models.Entities;

public class Contract
{
    public int Id { get; set; }

    [Required, Display(Name = "Client")]
    public int ClientId { get; set; }

    public Client? Client { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [Required]
    public ContractStatus Status { get; set; }

    [Required, Display(Name = "Service Level"), StringLength(100)]
    public string ServiceLevel { get; set; } = string.Empty;

    [Display(Name = "Signed Agreement File")]
    public string? SignedAgreementFileName { get; set; }

    [Display(Name = "Signed Agreement Path")]
    public string? SignedAgreementPath { get; set; }

    public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
