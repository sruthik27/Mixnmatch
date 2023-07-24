namespace FirstWeb.Models;

using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[FirestoreData]
public class UserWebCombo
{
    [FirestoreProperty]
    public string Fg { get; set; }

    [FirestoreProperty]
    public string Bg { get; set; }
}

public class FirestoreModel
{
    private FirestoreDb db;

    public FirestoreModel(string projectId)
    {
        FirestoreDbBuilder builder = new FirestoreDbBuilder
        {
            ProjectId = projectId
        };
        db = builder.Build();
    }

    public async Task StoreFgBgValues(string userEmail, string fgValue, string bgValue)
    {
        try
        {
            CollectionReference webcombosCollectionRef = db.Collection("favourites").Document("webcombos").Collection(userEmail);
            DocumentReference userFavoritesDocRef = webcombosCollectionRef.Document("user_favorites");

            // Create an instance of UserWebCombo to store fg and bg values
            UserWebCombo userWebCombo = new UserWebCombo
            {
                Fg = fgValue,
                Bg = bgValue
            };

            // Save the data to the document
            await userFavoritesDocRef.SetAsync(userWebCombo);
        }
        catch (Exception ex)
        {
            // Handle exceptions here or pass them to the calling method
            throw ex;
        }
    }

    public async Task<UserWebCombo> GetFgBgValues(string userEmail)
    {
        try
        {
            CollectionReference webcombosCollectionRef = db.Collection("favourites").Document("webcombos").Collection(userEmail);
            DocumentReference userFavoritesDocRef = webcombosCollectionRef.Document("user_favorites");

            // Retrieve the data from the document
            DocumentSnapshot snapshot = await userFavoritesDocRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                // Map the data to UserWebCombo object
                UserWebCombo userWebCombo = snapshot.ConvertTo<UserWebCombo>();
                return userWebCombo;
            }

            // If the document doesn't exist, return null or handle accordingly
            return null;
        }
        catch (Exception ex)
        {
            // Handle exceptions here or pass them to the calling method
            throw ex;
        }
    }
}