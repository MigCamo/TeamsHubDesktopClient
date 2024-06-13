
namespace TeamsHubDesktopClient.DTOs;

public partial class UserValidationResponse
{
    public bool IsValid {get; set;}
    public string Token {get; set;}
    public User? User {get; set;}
}