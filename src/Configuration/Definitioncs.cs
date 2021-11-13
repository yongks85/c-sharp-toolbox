

namespace Configuration;

public interface ISettingDefinition
{
    Dictionary<string, SettingNode> LoadDefinition();
}
