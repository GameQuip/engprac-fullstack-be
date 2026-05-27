using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Job;

public record JobUpdateRequest(
    [Required(ErrorMessage = "Title is required")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
    [MaxLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    string Title,

    [Required(ErrorMessage = "Company name is required")]
    [MinLength(2, ErrorMessage = "Company name must be at least 2 characters")]
    [MaxLength(200, ErrorMessage = "Company name must not exceed 200 characters")]
    string CompanyName,

    [MaxLength(1000, ErrorMessage = "Description must not exceed 1000 characters")]
    string? Description,

    [MaxLength(200, ErrorMessage = "Location must not exceed 200 characters")]
    string? Location,

    [Required(ErrorMessage = "Status is required")]
    [RegularExpression("^(Open|Closed|Draft|Active|Inactive)$", ErrorMessage = "Status must be Open, Closed, Draft, Active, or Inactive")]
    string Status,

    [Required(ErrorMessage = "Type is required")]
    [RegularExpression("^(Internship|Full-time|Part-time|Remote|Contract)$", ErrorMessage = "Type must be Internship, Full-time, Part-time, Remote, or Contract")]
    string Type,

    [Range(1, int.MaxValue, ErrorMessage = "RelatedUserId must be greater than 0")]
    int RelatedUserId
);