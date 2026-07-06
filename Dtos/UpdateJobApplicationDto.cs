using System.ComponentModel.DataAnnotations;

namespace JobTrackerApi.Dtos;

public class UpdateJobApplicationDto
{
    [Required]
    [MaxLength(100)]
    public string Company { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Role { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Applied";

    public DateTime AppliedDate { get; set; } = DateTime.Now;

    [MaxLength(500)]
    public string? Notes { get; set; }
}