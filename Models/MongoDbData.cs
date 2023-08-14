using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FirstWeb.Models;

using MongoDB.Driver;

public class MongoDbData
{
    private readonly IMongoCollection<StoredCombo> _favoritesCollection;
    private readonly IMongoCollection<StoredTriCombo> _favoritesTrioCollection;

    public MongoDbData(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("Mixnmatch");
        _favoritesCollection = database.GetCollection<StoredCombo>("favourites");
        _favoritesTrioCollection = database.GetCollection<StoredTriCombo>("favouritetrios");
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
    
    public class StoredTriCombo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Ensure mapping to ObjectId
        public string Id { get; set; } // Use string type for ObjectId
        public string UserEmail { get; set; }
        public string BgColor { get; set; }
        public string TextColor { get; set; }
        public string NaviColor { get; set; }
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
    
    public async Task SaveTriCombo(string userEmail, string bgColor, string textColor,string naviColor)
    {
        try
        {
            var StoredTriCombo = new StoredTriCombo
            {
                UserEmail = userEmail,
                BgColor = bgColor,
                TextColor = textColor,
                NaviColor = naviColor
            };
            await _favoritesTrioCollection.InsertOneAsync(StoredTriCombo);
            Console.WriteLine("added!");
        }
        catch (Exception ex)
        {
            // Handle exceptions or log the error
            throw new Exception("Error saving new color combo.", ex);
        }
    }

    public async Task<int> GetCount(string userEmail)
    {
        try
        {
            var filter1 = Builders<StoredCombo>.Filter.Eq(c => c.UserEmail, userEmail);
            var count1 = await _favoritesCollection.CountDocumentsAsync(filter1);
            var filter2 = Builders<StoredTriCombo>.Filter.Eq(c => c.UserEmail, userEmail);
            var count2 = await _favoritesTrioCollection.CountDocumentsAsync(filter2);
            return (int)count1 + (int)count2;
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
    
    public async Task<List<StoredTriCombo>> GetAllTriCombos(string userEmail)
    {
        try
        {
            // Search for combos where UserEmail matches the provided userEmail
            var filter = Builders<StoredTriCombo>.Filter.Eq(c=>c.UserEmail, userEmail);
            var userCombos = await _favoritesTrioCollection.Find(filter).ToListAsync();
            return userCombos;
        }
        catch (Exception ex)
        {
            // Handle exceptions or log the error
            throw new Exception("Error retrieving user combos.", ex);
        }
    }

}