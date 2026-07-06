namespace JobTrackerApi.Dtos;

public class JobApplicationResponseDto
{
    public int Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedDate { get; set; }
    public string? Notes { get; set; }
}