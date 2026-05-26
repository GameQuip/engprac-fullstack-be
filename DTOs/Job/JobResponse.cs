namespace Backend.DTOs.Job;

public record JobResponse(
    int Id,
    string Title,
    string CompanyName,
    string? Description,
    string? Location,
    DateTime CreatedAt
);
