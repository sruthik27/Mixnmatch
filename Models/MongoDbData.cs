using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FirstWeb.Models;

using MongoDB.Driver;

public class MongoDbData
{
    private readonly IMongoCollection<StoredCombo> _favoritesCollection;

    public MongoDbData(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("Mixnmatch");
        _favoritesCollection = database.GetCollection<StoredCombo>("favourites");
    }

    public class StoredCombo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Ensure mapping to ObjectId
        public string Id { get; set; } // Use string type for ObjectId
        public string UserEmail { get; set; }
        public string FgColor { get; set; }
        public string BgColor { get; set; }
    }

    public async Task SaveCombo(string userEmail, string fgColor, string bgColor)
    {
        try
        {
            var StoredCombo = new StoredCombo
            {
                UserEmail = userEmail,
                FgColor = fgColor,
                BgColor = bgColor
            };
            await _favoritesCollection.InsertOneAsync(StoredCombo);
            Console.WriteLine("added!");
        }
        catch (Exception ex)
        {
            // Handle exceptions or log the error
            throw new Exception("Error saving new color combo.", ex);
        }
    }

    public async Task<int> GetComboCount(string userEmail)
    {
        try
        {
            var filter = Builders<StoredCombo>.Filter.Eq(c => c.UserEmail, userEmail);
            var count = await _favoritesCollection.CountDocumentsAsync(filter);
            return (int)count;
        }
        catch (Exception ex)
        {
            // Handle exceptions or log the error
            throw new Exception("Error retrieving combo count.", ex);
        }
    }
    
    public async Task<List<StoredCombo>> GetAllCombos(string userEmail)
    {
        try
        {
            // Search for combos where UserEmail matches the provided userEmail
            var filter = Builders<StoredCombo>.Filter.Eq(c=>c.UserEmail, userEmail);
            var userCombos = await _favoritesCollection.Find(filter).ToListAsync();
            return userCombos;
        }
        catch (Exception ex)
        {
            // Handle exceptions or log the error
            throw new Exception("Error retrieving user combos.", ex);
        }
    }

}