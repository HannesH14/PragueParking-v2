using System.IO;
using System.Text.Json;

namespace PragueParking.Data;

public class MinaFiler
{
    public string[] LäsRader(string filnamn)
    {
        return File.ReadAllLines(filnamn);
    }

    public bool FinnsFil(string filnamn)
    {
        return File.Exists(filnamn);
    }

    public T? LäsJson<T>(string filnamn)
    {
        string json = File.ReadAllText(filnamn);
        return JsonSerializer.Deserialize<T>(json);
    }

    public void SparaJson<T>(string filnamn, T data)
    {
        string json = JsonSerializer.Serialize(data,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filnamn, json);
    }
}
