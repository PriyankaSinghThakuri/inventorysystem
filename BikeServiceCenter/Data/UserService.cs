// This class provides methods for managing users
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace BikeServiceCenter.Data;

public static class UserService
{
    // The default username for the seed user
    public const string SeedUsername = "admin";

    // The default password for the seed user
    public const string SeedPassword = "admin";

    // Saves the list of users to the users file
    private static void SaveAll(List<User> users)
    {
        // Get the path to the users file
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appUsersFilePath = Utils.GetAppUsersFilePath();

        // Create the directory if it doesn't exist
        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        // Serialize the users to a JSON string
        var json = JsonSerializer.Serialize(users);

        // Save the JSON string to the users file
        File.WriteAllText(appUsersFilePath, json);
    }

    // Returns the list of all users
    public static List<User> GetAll()
    {
        // Get the path to the users file
        string appUsersFilePath = Utils.GetAppUsersFilePath();

        // If the file doesn't exist, return an empty list
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        // Read the contents of the users file
        var json = File.ReadAllText(appUsersFilePath);

        // Deserialize the JSON string to a list of users
        return JsonSerializer.Deserialize<List<User>>(json);
    }

    // Creates a new user
    public static List<User> Create(Guid userId, string username, string password, Role role)
    {
        // Get the list of all users
        List<User> users = GetAll();

        // Check if the username already exists
        bool usernameExists = users.Any(x => x.Username == username);

        if (usernameExists)
        {
            // If the username already exists, throw an exception
            throw new Exception("Username already exists.");
        }

        // Add the new user to the list
        users.Add(
            new User
            {
                Username = username,
                PasswordHash = Utils.HashSecret(password),
                Role = role,
                CreatedBy = userId
            }
        );

        // Save the list of users
        SaveAll(users);

        // Return the updated list of users
        return users;
    }

    // Seeds the users with the default admin user if it doesn't exist
    public static void SeedUsers()
    {
        // Get the list of all users
        var users = GetAll();

        // Check if there is an admin user in the list
        var adminUser = users.FirstOrDefault(x => x.Role == Role.Admin);

        if (adminUser == null)
        {
            // If there is no admin user, create one with the default username and password
            Create(Guid.Empty, SeedUsername, SeedPassword, Role.Admin);
        }
    }


    // Gets a user by their ID
    public static User GetById(Guid id)
    {
        // Get the list of all users
        List<User> users = GetAll();

        // Return the user with the specified ID
        return users.FirstOrDefault(x => x.Id == id);
    }


    // Deletes a user by their ID
    public static List<User> Delete(Guid id)
    {
        // Get the list of all users
        List<User> users = GetAll();

        // Find the user with the specified ID
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            // If the user is not found, throw an exception
            throw new Exception("User not found.");
        }

        // Delete the items associated with the user
        ItemsService.DeleteByUserId(id);

        // Remove the user from the list
        users.Remove(user);

        // Save the updated list of users
        SaveAll(users);

        // Return the updated list of users
        return users;
    }


    // Logs a user in
    public static User Login(string username, string password)
    {
        // The error message to return if the login fails
        var loginErrorMessage = "Invalid username or password.";

        // Get the list of all users
        List<User> users = GetAll();

        // Find the user with the specified username
        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            // If the user is not found, throw an exception
            throw new Exception(loginErrorMessage);
        }

        // Check if the password is valid
        bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

        if (!passwordIsValid)
        {
            // If the password is invalid, throw an exception
            throw new Exception(loginErrorMessage);
        }

        // Return the user if the login is successful
        return user;
    }



    // Changes the password of a user
    public static User ChangePassword(Guid id, string currentPassword, string newPassword)
    {
        // Check if the new password is different from the current password
        if (currentPassword == newPassword)
        {
            throw new Exception("New password must be different from current password.");
        }

        // Get the list of all users
        List<User> users = GetAll();

        // Find the user with the specified ID
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            // If the user is not found, throw an exception
            throw new Exception("User not found.");
        }

        // Check if the current password is valid
        bool passwordIsValid = Utils.VerifyHash(currentPassword, user.PasswordHash);

        if (!passwordIsValid)
        {
            // If the current password is invalid, throw an exception
            throw new Exception("Incorrect current password.");
        }

        // Set the new password for the user
        user.PasswordHash = Utils.HashSecret(newPassword);

        // Set the HasInitialPassword flag to false
        user.HasInitialPassword = false;

        // Save the updated list of users
        SaveAll(users);

        // Return the user with the updated password
        return user;
    }



}

