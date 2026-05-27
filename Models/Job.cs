namespace Backend.Models;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string Type { get; set; } = "Full-time";
    public int RelatedUserId { get; set; }
    public User? RelatedUser { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
