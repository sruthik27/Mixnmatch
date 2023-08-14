using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace FirstWeb.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly IMongoCollection<User> _usersCollection;

        public SignUpModel(IMongoClient mongoClient)
        {
            // Replace "Mixnmatch" and "Users" with your actual database and collection names
            var database = mongoClient.GetDatabase("Mixnmatch");
            _usersCollection = database.GetCollection<User>("Users");
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost(string email, string password)
        {
            // Check if the email is already registered
            var user = _usersCollection.Find(u => u.Email == email).FirstOrDefault();
            if (user != null)
            {
                // User already exists, show an error message or redirect to the login page
                return RedirectToPage("Login");
            }

            // If the user does not exist, create a new user document and insert it into the collection
            var newUser = new User
            {
                Email = email,
                Password = password // You should never store plain passwords in production; consider hashing them
            };

            _usersCollection.InsertOne(newUser);

            // Redirect to a confirmation page or any other page you want
            return RedirectToPage("SignUpSuccess");
        }
    }

    // Create a simple User model to store user data
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}