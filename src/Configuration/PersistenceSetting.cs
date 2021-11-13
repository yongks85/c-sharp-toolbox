using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configuration;

public interface IPersistenceSetting
{
    Task<List<SerializedSetting>> Load();
    Task Save(List<SerializedSetting> setting);
}

internal class PersistInFile: IPersistenceSetting
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options;

    public PersistInFile(string filePath)
    {
        _filePath = filePath;
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<List<SerializedSetting>> Load()
    {
        using var stream = File.OpenRead(_filePath);
        var result = await JsonSerializer.DeserializeAsync<List<SerializedSetting>>(stream, _options);
        return result!;
    }

    public async Task Save(List<SerializedSetting> setting)
    {
        using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, setting, _options);
    }
}


internal class PersistInMemory : IPersistenceSetting
{
    private readonly JsonSerializerOptions _options;
    public string Serialized { get; private set; } = string.Empty;

    public PersistInMemory()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public Task<List<SerializedSetting>> Load()
    {
        var result = JsonSerializer.Deserialize<List<SerializedSetting>>(Serialized, _options);
        return Task.FromResult(result)!;
    }

    public Task Save(List<SerializedSetting> setting)
    {
        Serialized = JsonSerializer.Serialize(setting, _options);
        return Task.CompletedTask;
    }
}

