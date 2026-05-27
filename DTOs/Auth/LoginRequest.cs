using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Auth;

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password
);
