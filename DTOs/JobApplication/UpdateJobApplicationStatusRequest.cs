using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.JobApplication;

public record UpdateJobApplicationStatusRequest(
    [Required(ErrorMessage = "Status is required")]
    [RegularExpression("^(Passed|Failed)$", ErrorMessage = "Status must be Passed or Failed")]
    string Status
);