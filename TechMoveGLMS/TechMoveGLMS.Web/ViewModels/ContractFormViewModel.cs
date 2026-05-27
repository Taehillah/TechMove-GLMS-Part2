using System.ComponentModel.DataAnnotations;
using TechMoveGLMS.Web.Models.Enums;

namespace TechMoveGLMS.Web.ViewModels;

public class ContractFormViewModel : IValidatableObject
{
    public int Id { get; set; }

    [Required, Display(Name = "Client")]
    public int ClientId { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required, DataType(DataType.Date), Display(Name = "End Date")]
    public DateTime EndDate { get; set; } = DateTime.Today.AddYears(1);

    [Required]
    public ContractStatus Status { get; set; } = ContractStatus.Draft;

    [Required, Display(Name = "Service Level"), StringLength(100)]
    public string ServiceLevel { get; set; } = string.Empty;

    [Display(Name = "Signed Agreement PDF")]
    public IFormFile? SignedAgreement { get; set; }

    public string? ExistingSignedAgreementFileName { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate < StartDate)
        {
            yield return new ValidationResult(
                "End Date must be on or after Start Date.",
                [nameof(EndDate)]);
        }
    }
}
