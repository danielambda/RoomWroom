using System.Text.Json;
using RoomWroom.Grocery;

namespace RoomWroom.Database.Grocery;

public class FileReceiptsRepository : IReceiptsRepository
{
    private const string RECEIPTS_FILE = "Receipts.json";
    private const string TRANSLATED_NAMES_FILE = "TranslatedNames.json";

    private readonly Dictionary<string, Receipt> _receipts;
    private readonly Dictionary<string, string> _translatedNames;

    public FileReceiptsRepository()
    {
        _receipts = InitReceipts();
        _translatedNames = InitTranslatedNames();
    }

    private Dictionary<string, Receipt> InitReceipts()
    {
        using Stream stream = new FileStream(RECEIPTS_FILE, FileMode.OpenOrCreate, FileAccess.Read);
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        _ = stream.Read(buffer);

        if (buffer.Length == 0)
            return [];

        Utf8JsonReader jsonReader = new(buffer);
        JsonElement jsonElement = JsonElement.ParseValue(ref jsonReader);
        Dictionary<string, Receipt>? receipts = jsonElement.Deserialize<Dictionary<string, Receipt>>();

        return receipts ?? [];
    }

    private Dictionary<string, string> InitTranslatedNames()
    {
        using Stream stream = new FileStream(TRANSLATED_NAMES_FILE, FileMode.OpenOrCreate, FileAccess.Read);
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        _ = stream.Read(buffer);

        if (buffer.Length == 0)
            return [];

        Utf8JsonReader jsonReader = new(buffer);
        JsonElement jsonElement = JsonElement.ParseValue(ref jsonReader);
        Dictionary<string, string>? translatedNames = jsonElement.Deserialize<Dictionary<string, string>>();

        return translatedNames ?? [];
    }

    public Receipt? Get(string qrText) => _receipts.GetValueOrDefault(qrText);

    public void Add(string qrText, Receipt receipt)
    {
        _receipts.TryAdd(qrText, receipt);
        UpdateReceiptsFile();
    }

    public string? GetTranslatedName(string name) => _translatedNames.GetValueOrDefault(name);

    public void AddTranslatedName(string name, string translatedName)
    {
        _translatedNames.TryAdd(name, translatedName);
        UpdateTranslatedNamesFile();
    }

    private void UpdateReceiptsFile()
    {
        using Stream stream = new FileStream(RECEIPTS_FILE, FileMode.OpenOrCreate, FileAccess.Write);
        string json = JsonSerializer.Serialize(_receipts);
        Span<byte> buffer = System.Text.Encoding.UTF8.GetBytes(json);

        stream.Write(buffer);
    }

    private void UpdateTranslatedNamesFile()
    {
        using Stream stream = new FileStream(TRANSLATED_NAMES_FILE, FileMode.OpenOrCreate, FileAccess.Write);
        string json = JsonSerializer.Serialize(_translatedNames);
        Span<byte> buffer = System.Text.Encoding.UTF8.GetBytes(json);

        stream.Write(buffer);
    }
}