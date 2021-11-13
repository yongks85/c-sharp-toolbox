using System.Text.Json.Serialization;
using Ardalis.GuardClauses;

namespace Configuration;

public record SettingInfo
{
    public SettingInfo(string defaultValue)
    {
        Guard.Against.NullOrWhiteSpace(defaultValue, "Setting Default Value");
        DefaultValue = defaultValue;
    }

    /// <summary>
    /// Will not be saved, but can be loaded
    /// Will not show up when get all settings
    /// Default: True
    /// Usecase: Preview/development setting
    /// </summary>
    public bool Visible { get; init; } = true;
        
    /// <summary>
    /// Will not be saved. Value cannot be changed
    /// Will show up when get all settings
    /// Default: False
    /// Usecase: Finialized Setting live production setting
    /// </summary>
    public bool ReadOnly { get; init; } = false;

    public string Description { get; init; } = string.Empty;

    public string DefaultValue { get; }
}

public record SerializedSetting
{
    public string Name { get; init; }
        
    public string Value { get; init; }
    
    [JsonIgnore]
    public bool ReadOnly { get; init; }
}

public class SettingNode
{
    private string _value;
    public SettingInfo Info { get; init; }
    
    public string Value { 
        get=> string.IsNullOrEmpty(_value) ? Info.DefaultValue :_value;
        set
        {
            if (!Info.ReadOnly) _value = value;
        }

    }
    public IDictionary<string, SettingNode> Children { get; init; }

    public static implicit operator string(SettingNode node) => node.Value;

    public SettingNode this[string key] => Children[key];
}