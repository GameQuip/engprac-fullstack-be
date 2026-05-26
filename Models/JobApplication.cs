namespace Backend.Models;

public class JobApplication
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = "Applied";
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public Job? Job { get; set; }
    public User? User { get; set; }
}
