using System.Security.Cryptography;

namespace BikeServiceCenter.Data;

public static class Utils
{
    // Delimiter used to separate segments of the hashed password string
    private const char _segmentDelimiter = ':';

    // Hashes the input string and returns a string with the following format:
    // [HashedPassword]:[Salt]:[Iterations]:[HashAlgorithm]
    public static string HashSecret(string input)
    {
        // Size of the salt in bytes
        var saltSize = 16;
        // Number of iterations for the PBKDF2 algorithm
        var iterations = 100_000;
        // Size of the derived key in bytes
        var keySize = 32;
        // Hash algorithm to use
        HashAlgorithmName algorithm = HashAlgorithmName.SHA256;

        // Generate a random salt
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);

        // Use PBKDF2 to derive a key from the input and the salt
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        // Return the hashed password string with the salt, iterations, and algorithm included
        return string.Join(
            _segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );
    }

    // Verifies that the input string matches the hashed password string
    public static bool VerifyHash(string input, string hashString)
    {
        // Split the hashed password string into segments
        string[] segments = hashString.Split(_segmentDelimiter);
        // Convert the hashed password segment to a byte array
        byte[] hash = Convert.FromHexString(segments[0]);
        //Convert the salted password segment to a byte array
        byte[] salt = Convert.FromHexString(segments[1]);
        //  the number of iterations, which is parsed to an integer and stored in the iterations variable.
        int iterations = int.Parse(segments[2]);
       // the hashing algorithm used, which is stored in the algorithm variable.
        HashAlgorithmName algorithm = new(segments[3]);
        // hashed input using PBKDF2 (Password-Based Key Derivation Function 2) 
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );
        //the inputHash is compared to the original hash using a constant-time comparison function to prevent timing attacks.
        //If the two hashes are equal, the method returns true; otherwise, it returns false.
        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }

    //returns the file path to the application's directory.
    public static string GetAppDirectoryPath()
    {
        // combine the path to the user's application data folder (obtained using Environment.GetFolderPath)
        // with the subdirectory name "Bike-Service-Center"
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Bike-Service-Center"
        );
    }

    //get the file path to the application's directory
    public static string GetAppUsersFilePath()
    {
        //combines the file path with the file name "users.json" using the Path.Combine method.
        //The resulting file path is returned.
        return Path.Combine(GetAppDirectoryPath(), "users.json");
    }

    //get the file path to the application's directory
    public static string GetItemsFilePath()
    {
        //combines the file path with the file name "_items.json" using the Path.Combine method.
        //The resulting file path is returned.
        return Path.Combine(GetAppDirectoryPath(), "_items.json");
    }

    //get the file path to the application's directory
    public static string GetApprovedFilePath()
    {
        //combines the file path with the file name "approved.json" using the Path.Combine method.
        //The resulting file path is returned.
        return Path.Combine(GetAppDirectoryPath(), "approved.json");
    }
    //get the file path to the application's directory
    public static string GetWithdrawlFilePath()
    {
        //combines the file path with the file name "withdrawl.json" using the Path.Combine method.
        //The resulting file path is returned.
        return Path.Combine(GetAppDirectoryPath(), "withdrawl.json");
    }
}
