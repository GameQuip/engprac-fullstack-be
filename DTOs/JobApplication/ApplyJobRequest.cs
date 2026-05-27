using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.JobApplication;

public record ApplyJobRequest(
    [Range(1, int.MaxValue, ErrorMessage = "JobId must be greater than 0")]
    int JobId
);