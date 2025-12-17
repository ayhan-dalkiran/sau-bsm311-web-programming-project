namespace GymWebsite.Models.Dto;

public class UserLogin {
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool Persistent { get; set; } = false;
}
