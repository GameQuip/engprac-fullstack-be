namespace Backend.DTOs.Job;

public record JobCreateRequest(
    string Title,
    string CompanyName,
    string? Description,
    string? Location
);