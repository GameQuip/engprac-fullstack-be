namespace Backend.DTOs.Job;

public record JobUpdateRequest(
    string Title,
    string CompanyName,
    string? Description,
    string? Location,
    string Status,
    string Type,
    int RelatedUserId
);
