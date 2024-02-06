using System.Text.Json;

namespace VerticalSliceModularMonolith.Shared.Utils;

public static class JsonUtils
{
    public static T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(
            json,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true }
        );
    }

    public static T? Deserialize<T>(JsonDocument json)
    {
        return json.Deserialize<T>(
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true }
        );
    }

    public static string Serialize(object json)
    {
        return JsonSerializer.Serialize(
            json,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true }
        );
    }

    public static JsonDocument SerializeToDocument(object json)
    {
        return JsonSerializer.SerializeToDocument(
            json,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true }
        );
    }
}
