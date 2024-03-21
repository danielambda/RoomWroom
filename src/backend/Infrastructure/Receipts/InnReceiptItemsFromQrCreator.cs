using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Receipts.Interfaces;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate.ValueObjects;

namespace Infrastructure.Receipts;

public class InnReceiptItemsFromQrCreator : IReceiptItemsFromQrCreator
{
    private const string TAX_SERVICE_URL = "https://irkkt-mobile.nalog.ru:8888/v2";
    private const string DEVICE_OS = "IOS";
    private const string CLIENT_VERSION = "2.9.0";
    private const string DEVICE_ID = "7C82010F-16CC-446B-8F66-FC4080C66521";
    private const string ACCEPT = "*/*";
    private const string USER_AGENT = "billchecker/2.9.0 (iPhone; iOS 13.6; Scale/2.00)";
    private const string ACCEPT_LANGUAGE = "ru-RU;q=1, en-US;q=0.9";
    private const string CLIENT_SECRET = "IyvrAbKt9h/8p6a7QPh8gpkXYQ4=";
    
    private static readonly string Inn = TryGetEnvironmentVariable("INN");
    private static readonly string Password = TryGetEnvironmentVariable("INN_PASSWORD");
    
    private static readonly HttpClient HttpClient = new();
    private static readonly JsonSerializerOptions ParseOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new ReceiptItemJsonConverter() }
    };

    private static string? _sessionId;
    private static string? _refreshToken;

    private static bool _initialized = false;

    private static bool IsRefreshable => false;

    public async Task<IEnumerable<ReceiptItem>?> CreateAsync(string qr, CancellationToken cancellationToken = default)
    {
        if (!_initialized)
            await Init(cancellationToken);

        if (await GetTickedId(qr, cancellationToken) is not { } ticketId)
            return null;

        JsonDocument jsonDocument = await GetJsonFromTickedId(ticketId);
        IEnumerable<ReceiptItem>? items = ParseReceiptItemsFromJson(jsonDocument);

        return items;
    }

    private static async Task Init(CancellationToken cancellationToken = default)
    {
        HttpClient.DefaultRequestHeaders.Add("Accept", ACCEPT);
        HttpClient.DefaultRequestHeaders.Add("Device-OS", DEVICE_OS);
        HttpClient.DefaultRequestHeaders.Add("Device-Id", DEVICE_ID);
        HttpClient.DefaultRequestHeaders.Add("clientVersion", CLIENT_VERSION);
        HttpClient.DefaultRequestHeaders.Add("Accept-Language", ACCEPT_LANGUAGE);
        HttpClient.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);

        if (IsRefreshable)
            await Refresh(cancellationToken);
        else 
            await Authorize(cancellationToken);

        _initialized = true;
    }
    
    
    private static List<ReceiptItem>? ParseReceiptItemsFromJson(JsonDocument jsonDocument)
    {   
        JsonElement jsonItems = jsonDocument
            .RootElement
            .GetProperty("ticket")
            .GetProperty("document")
            .GetProperty("receipt")
            .GetProperty("items");

        var items = jsonItems.Deserialize<List<ReceiptItem>?>(ParseOptions);
        return items;
    }

    private static async Task<JsonDocument> GetJsonFromTickedId(string ticketId)
    {
        var url = $"{TAX_SERVICE_URL}/tickets/{ticketId}";

        HttpRequestMessage requestMessage = new(HttpMethod.Get, url);
        requestMessage.Headers.Add("sessionId", _sessionId);

        HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);
        string jsonOut = await response.Content.ReadAsStringAsync();

        JsonDocument jsonDocument = JsonDocument.Parse(jsonOut);
        return jsonDocument;
    }

    private static async Task Authorize(CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> payload = new()
        {
            { "inn", Inn },
            { "client_secret", CLIENT_SECRET },
            { "password", Password }
        };

        const string URL = $"{TAX_SERVICE_URL}/mobile/users/lkfl/auth";

        HttpContent content = JsonContent.Create(payload);            

        HttpResponseMessage response = await HttpClient.PostAsync(URL, content, cancellationToken);
        string jsonOut = await response.Content.ReadAsStringAsync(cancellationToken);
        IDictionary<string, string>? result = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonOut);

        _sessionId = result?["sessionId"];
        _refreshToken = result?["refresh_token"];
    }

    private static async Task Refresh(CancellationToken cancellationToken = default)
    {
        if (_refreshToken is null)
            throw new NullReferenceException(nameof(_refreshToken));
        
        Dictionary<string, string> jsonContent = new()
        {
            { "refresh_token", _refreshToken }
        };
        
        const string URL = $"{TAX_SERVICE_URL}/mobile/users/lkfl/refresh";

        HttpContent content = JsonContent.Create(jsonContent);
        HttpResponseMessage response = await HttpClient.PostAsync(URL, content, cancellationToken);
        string jsonOut = await response.Content.ReadAsStringAsync(cancellationToken);
        IDictionary<string, string>? result = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonOut);

        _sessionId = result?["sessionId"];
        _refreshToken = result?["refresh_token"];
    }
    
    private static async Task<string?> GetTickedId(string qr, CancellationToken cancellationToken = default)
    {
        while (true)
        {
            Dictionary<string, string> payload = new() { { "qr", qr } };

            const string URL = $"{TAX_SERVICE_URL}/ticket";

            HttpContent content = JsonContent.Create(payload);
            content.Headers.Add("sessionId", _sessionId);

            HttpResponseMessage response = await HttpClient.PostAsync(URL, content, cancellationToken);
            string jsonOut = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.StatusCode is HttpStatusCode.TooManyRequests)
                return null;
            
            if (response.StatusCode is HttpStatusCode.Unauthorized)
            {
                await Init(cancellationToken);
                continue;
            }

            var result = JsonSerializer.Deserialize<Dictionary<string, object>?>(jsonOut);
            return result?["id"].ToString();
        }
    }
    
    private static string TryGetEnvironmentVariable(string key)
    {
        string? value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
        return value ?? throw new NullReferenceException($"Environment variable {key} is null");
    }
}

file class ReceiptItemJsonConverter : JsonConverter<ReceiptItem>
{
    public override ReceiptItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonReceiptItem = JsonSerializer.Deserialize<JsonReceiptItem?>(ref reader, options);

        return jsonReceiptItem is null
            ? null
            : new ReceiptItem(
                jsonReceiptItem.Name,
                new Money(jsonReceiptItem.Price / 100m, Currency.Rub),
                jsonReceiptItem.Quantity
            );
    }

    public override void Write(Utf8JsonWriter writer, ReceiptItem value, JsonSerializerOptions options)
    {
    }
}

file sealed record JsonReceiptItem(string Name, int Price, decimal Quantity);
