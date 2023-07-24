using System.Text;

namespace FirstWeb.Models;

public class FirebaseData
{
    private readonly HttpClient _httpClient;
    string url = "https://global-justice-367016-default-rtdb.firebaseio.com/count.json";
    public FirebaseData()
    {
        _httpClient = new HttpClient();
    }

    public int FetchDataFromFirebase()
    {
        var response =  _httpClient.GetStringAsync(url).Result;
        return Convert.ToInt32(response);
    }

    public async Task Increment()
    {
        var count = FetchDataFromFirebase();
        count++;
        var content = new StringContent(count.ToString(), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
    }
}