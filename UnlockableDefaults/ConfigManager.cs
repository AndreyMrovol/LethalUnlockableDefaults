using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;

namespace UnlockOnStart;

public class ConfigManager
{
  public static ConfigManager Instance { get; private set; }

  public static void Init(ConfigFile config)
  {
    Instance = new ConfigManager(config);
  }

  public static ConfigFile configFile { get; private set; }

  public static Dictionary<string, ConfigEntry<float>> Positions = new Dictionary<string, ConfigEntry<float>>();

  // public static Dictionary<string, ConfigEntry<bool>> InStorage = new Dictionary<string, ConfigEntry<bool>>();

  private ConfigManager(ConfigFile config)
  {
    configFile = config;
  }

  public static void Add(string unlockableName, string key, float value)
  {
    if (Positions.TryGetValue(unlockableName + key, out var configEntry))
    {
      Positions[unlockableName + key].BoxedValue = value;
      return;
    }

    Positions[unlockableName + key] = configFile.Bind(unlockableName, key, value, key);
  }

  public static float TryGet(string unlockableName, string key)
  {
    if (Positions.TryGetValue(unlockableName + key, out var configEntry))
    {
      return configEntry.Value;
    }
    else
    {
      Add(unlockableName, key, 0f);
    }

    return 0f;
  }

  public static void InitialBind(string unlockableName, string key)
  {
    Positions[unlockableName + key] = configFile.Bind(unlockableName, key, 0f, key);
  }
}
