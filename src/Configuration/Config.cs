namespace Configuration;

/// <summary>
/// Setting Main class, Eg. Application Setting, sub domain setting
/// </summary>
public class Config
{
    private readonly IPersistenceSetting _persistenceSetting;
    private readonly Dictionary<string, SettingNode> _settings;

    public Config(ISettingDefinition definitionLoader, IPersistenceSetting persistence)
    {
        _persistenceSetting = persistence;
        _settings = definitionLoader.LoadDefinition();
    }

    public async Task LoadSettings()
    {
        var settings = await _persistenceSetting.Load();
        foreach (var serialized in settings)
        {
            var toUpdate = FindNode(_settings, serialized.Name.Split('.'));
            if (toUpdate != null && !toUpdate.Info.ReadOnly) toUpdate.Value = serialized.Value;
        }
    }
    
    public async Task SaveSettings()
    {
        var setting = ToSerializable(_settings);
        setting = setting.Where(s => !s.ReadOnly).ToList();
        await _persistenceSetting.Save(setting.ToList());
    }

    public void RestoreDefault() => RestoreDefaultInternal(_settings);
    
    public string this[string key]
    {
        get => _settings.SingleOrDefault(n => n.Key == key).Value;
        set => _settings[key].Value = value;
    }

    private static SettingNode? FindNode(IDictionary<string, SettingNode> settings, IReadOnlyList<string> namespaces)
    {
        var index = 0;
        while (namespaces.Count > index)
        {
            if (!settings.ContainsKey(namespaces[index])) return null;
            var setting = settings[namespaces[index]];
            if (namespaces.Count == index + 1) return setting;

            index++;
            settings = setting.Children;
        }

        return null;
    }

    private static void RestoreDefaultInternal(IDictionary<string, SettingNode> settings)
    {
        foreach (var setting in settings.Values)
        {
            setting.Value = setting.Info.DefaultValue;
            if (setting.Children.Any()) RestoreDefaultInternal(setting.Children);
        }
    }

    private static List<SerializedSetting> ToSerializable(IDictionary<string, SettingNode> settingNodes,
        string parentName = "")
    {
        var serialized = new List<SerializedSetting>();
        serialized.AddRange(
            settingNodes
                .Where(node => node.Value.Info.Visible)
                .Select(node =>
                new SerializedSetting
                {
                    Name = string.IsNullOrWhiteSpace(parentName) ? node.Key : parentName + "." + node.Key,
                    Value = node.Value,
                    ReadOnly = node.Value.Info.ReadOnly
                }
            )
        );

        foreach (var kvp in settingNodes)
        {
            if (kvp.Value.Children == null || !kvp.Value.Children.Any()) continue;

            serialized.AddRange(ToSerializable(kvp.Value.Children, kvp.Key));
        }

        return serialized;
    }
}
