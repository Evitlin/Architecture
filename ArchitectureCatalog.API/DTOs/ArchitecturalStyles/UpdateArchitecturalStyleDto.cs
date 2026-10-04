using System.ComponentModel.DataAnnotations;

namespace ArchitectureCatalog.API.DTOs.ArchitecturalStyles;

public class UpdateArchitecturalStyleDto : IValidatableObject
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string Description { get; set; } = null!;

    public int StartYear { get; set; }
    public int EndYear { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartYear > EndYear)
        {
            yield return new ValidationResult(
                "StartYear cannot be greater than EndYear.",
                [nameof(StartYear), nameof(EndYear)]);
        }
    }
}
