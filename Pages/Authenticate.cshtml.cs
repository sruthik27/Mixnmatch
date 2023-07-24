using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using FirstWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

[AllowAnonymous]
public class AuthenticateModel : PageModel
{
    private readonly IMongoCollection<BsonDocument> usersCollection;
    private readonly IConfiguration configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthenticateModel(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        this.configuration = configuration;
        _httpContextAccessor = httpContextAccessor;

        var connectionString = configuration.GetConnectionString("MongoDBConnection");
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("Mixnmatch");
        usersCollection = database.GetCollection<BsonDocument>("Users");
    }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [TempData]
    public string Message { get; set; }

    public void OnGet()
    {
        // This method is executed when the page is loaded (HTTP GET request).
    }

    public async Task<IActionResult> OnPost()
    {
        // This method is executed when the login form is submitted (HTTP POST request).

        // Search for the user with the provided username in the MongoDB collection.
        var filter = Builders<BsonDocument>.Filter.Eq("Username", Username);
        var user = usersCollection.Find(filter).FirstOrDefault();

        if (user != null)
        {
            // User with the provided username was found in the database.
            // Now, check if the password matches the stored hash.
            var passwordHash = user["PasswordHash"].AsString;
            if (VerifyPassword(Password, passwordHash))
            {
                // Passwords match, so create a ClaimsIdentity for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username),
                    // Add any other claims you want to include for the user
                    // For example, roles, custom claims, etc.
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // Issue the authentication cookie manually
                var authProperties = new AuthenticationProperties
                {
                    // Customize any properties of the cookie if needed
                    IsPersistent = false // Set this to true if you want a persistent cookie
                };
                _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties
                ).Wait();

                Console.WriteLine("Login successful!");
                return RedirectToPage("/saved");
            }
        }

        // Either the user was not found or the password was incorrect.
        // Display an error message on the login page and ask again.
        TempData["Message"] = "Invalid credentials. Please try again.";
        Console.WriteLine("Login failed!");
        return Page();
    }

    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        // Convert the stored hash back to bytes
        byte[] hashBytes = Convert.FromBase64String(storedHash);

        // Get the salt from the first 128 bits (16 bytes) of the hash
        byte[] salt = new byte[128 / 8];
        Array.Copy(hashBytes, 0, salt, 0, 128 / 8);

        // Generate the hash for the input password using the stored salt
        string inputHashedPassword = ConvertToHashedPassword(inputPassword, salt);

        // Compare the generated hash with the stored hash
        return storedHash == inputHashedPassword;
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