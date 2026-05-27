using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Auth;

public record LoginRequest(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(200, ErrorMessage = "Email must not exceed 200 characters")]
    string Email
);
