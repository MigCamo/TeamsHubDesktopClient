
namespace TeamHubServiceProjects.DTOs;

public partial class ProjectDTO
{
    public int IdProject { get; set; }

    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }
}
