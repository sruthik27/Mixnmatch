namespace FirstWeb.Pages;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class Register : PageModel
{
    private readonly IMongoCollection<User> _users;

    public Register(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("Mixnmatch");
        _users = database.GetCollection<User>("Users");
    }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        // Check if the username already exists
        var user = await _users.Find(u => u.Username == Username).FirstOrDefaultAsync();
        if (user != null)
        {
            ModelState.AddModelError("Username", "Username already exists.");
            TempData["Message"] = "Username already exists, try logging in or sign up with different name";
            return Page();
        }

        // Generate a salt for password hashing
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hash the password with the salt
        string hashedPassword = ConvertToHashedPassword(Password, salt);

        // Save the user to the database
        await _users.InsertOneAsync(new User
        {
            Username = Username,
            PasswordHash = hashedPassword
        });

        // Redirect to the login page after successful registration
        return RedirectToPage("/Authenticate");
    }

    private string ConvertToHashedPassword(string password, byte[] salt)
    {
        // Use PBKDF2 with 10000 iterations and SHA-256 for password hashing
        byte[] hashedBytes = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);

        // Combine the salt and hashed password bytes into one array
        byte[] combinedBytes = new byte[salt.Length + hashedBytes.Length];
        Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
        Array.Copy(hashedBytes, 0, combinedBytes, salt.Length, hashedBytes.Length);

        // Convert the combined bytes to Base64 string for storage
        return Convert.ToBase64String(combinedBytes);
    }

}