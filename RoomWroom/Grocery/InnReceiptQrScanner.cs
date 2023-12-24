using System.Net.Http.Json;
using System.Text.Json;

namespace RoomWroom.Grocery;

internal class InnReceiptQrScanner(string? inn, string? password) : IReceiptQrScanner
{
    private const string TAX_SERVICE_URL = "https://irkkt-mobile.nalog.ru:8888/v2";
    private const string DEVICE_OS = "IOS";
    private const string CLIENT_VERSION = "2.9.0";
    private const string DEVICE_ID = "7C82010F-16CC-446B-8F66-FC4080C66521";
    private const string ACCEPT = "*/*";
    private const string USER_AGENT = "billchecker/2.9.0 (iPhone; iOS 13.6; Scale/2.00)";
    private const string ACCEPT_LANGUAGE = "ru-RU;q=1, en-US;q=0.9";
    private const string CLIENT_SECRET = "IyvrAbKt9h/8p6a7QPh8gpkXYQ4=";
    
    private readonly string _inn = inn ?? throw new ArgumentNullException(nameof(inn));
    private readonly string _password = password ?? throw new ArgumentNullException(nameof(password));

    private readonly HttpClient _httpClient = new();

    private string? _sessionId;

    private bool _initialized = false;

    public async Task<Receipt> GetReceiptFromQrAsync(string qr)
    {
        if (!_initialized)
            await Init();

        if (await GetTickedId(qr) is not { } ticketId)
        {
            throw new NullReferenceException(nameof(ticketId));
        }

        JsonDocument jsonDocument = await GetJsonFromTickedId(ticketId);
        IEnumerable<GroceryItem>? items = ParseGroceryItemsFromJson(jsonDocument);
        Receipt receipt = new(items);

        return receipt;
    }

    private async Task Init()
    {
        _httpClient.DefaultRequestHeaders.Add("Accept", ACCEPT);
        _httpClient.DefaultRequestHeaders.Add("Device-OS", DEVICE_OS);
        _httpClient.DefaultRequestHeaders.Add("Device-Id", DEVICE_ID);
        _httpClient.DefaultRequestHeaders.Add("clientVersion", CLIENT_VERSION);
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", ACCEPT_LANGUAGE);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);

        _sessionId = await GetSessionId();

        _initialized = true;
    }

    private static IEnumerable<GroceryItem>? ParseGroceryItemsFromJson(JsonDocument jsonDocument)
    {
        JsonElement jsonItems = jsonDocument.RootElement.GetProperty("ticket").GetProperty("document")
            .GetProperty("receipt").GetProperty("items");

        JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
        List<GroceryItem>? items = jsonItems.Deserialize<List<GroceryItem>?>(options);
        return items;
    }

    private async Task<JsonDocument> GetJsonFromTickedId(string ticketId)
    {
        string url = $"{TAX_SERVICE_URL}/tickets/{ticketId}";

        HttpRequestMessage requestMessage = new(HttpMethod.Get, url);
        requestMessage.Headers.Add("sessionId", _sessionId);

        HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
        string jsonOut = await response.Content.ReadAsStringAsync();

        JsonDocument jsonDocument = JsonDocument.Parse(jsonOut);
        return jsonDocument;
    }

    private async Task<string?> GetSessionId()
    {
        Dictionary<string, string> payload = new()
        {
            { "inn", _inn },
            { "client_secret", CLIENT_SECRET },
            { "password", _password }
        };

        const string URL = $"{TAX_SERVICE_URL}/mobile/users/lkfl/auth";

        HttpContent content = JsonContent.Create(payload);            

        HttpResponseMessage response = await _httpClient.PostAsync(URL, content);
        string jsonOut = await response.Content.ReadAsStringAsync();
        IDictionary<string, string>? result = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonOut);

        return result?["sessionId"];
    }

    private async Task<string?> GetTickedId(string qr)
    {
        while (true)
        {
            Dictionary<string, string> payload = new() { { "qr", qr } };

            const string URL = $"{TAX_SERVICE_URL}/ticket";

            HttpContent content = JsonContent.Create(payload);
            content.Headers.Add("sessionId", _sessionId);

            HttpResponseMessage response = await _httpClient.PostAsync(URL, content);
            string jsonOut = await response.Content.ReadAsStringAsync();

            if (jsonOut != "Unauthorized")
            {
                IDictionary<string, object>? result = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonOut);

                return result?["id"].ToString();
            }

            await Init();
        }
    }
}