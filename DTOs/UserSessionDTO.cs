
namespace TeamsHubDesktopClient.DTOs;

public partial class UserSessionDTO
{
    public User User { get; set; }
    public string Token { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}