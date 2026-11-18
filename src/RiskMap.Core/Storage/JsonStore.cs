using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RiskMap.Core.Storage;

/// <summary>Lecture et écriture des fichiers JSON (résultats de scan, données d'exemple, règles).</summary>
public static class JsonStore
{
    public static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // garde les accents lisibles
        Converters = { new JsonStringEnumConverter() },
    };

    public static T Load<T>(string path) =>
        JsonSerializer.Deserialize<T>(File.ReadAllText(path), Options)
        ?? throw new InvalidDataException($"Fichier JSON vide ou invalide : {path}");

    public static void Save<T>(string path, T value)
    {
        var dir = Path.GetDirectoryName(Path.GetFullPath(path));
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(path, Serialize(value));
    }

    public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    /// <summary>Copie profonde d'un objet (utile pour ne pas modifier les données d'exemple).</summary>
    public static T Clone<T>(T value) =>
        JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, Options), Options)!;
}
