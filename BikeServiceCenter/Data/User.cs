// This class represents a user
namespace BikeServiceCenter.Data;

public class User
{
    // The unique ID of the user
    public Guid Id { get; set; } = Guid.NewGuid();

    // The username of the user
    public string Username { get; set; }

    // The password hash of the user
    public string PasswordHash { get; set; }

    // The role of the user (either User or Admin)
    public Role Role { get; set; }

    // A flag indicating whether the user has a temporary initial password
    public bool HasInitialPassword { get; set; } = true;

    // The date when the user was created
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // The ID of the user who created the user
    public Guid CreatedBy { get; set; }
}
