namespace Backend.DTOs.Auth;

public record AuthResponse(
    string Token,
    int UserId,
    string FullName,
    string Email,
    string Role
);
