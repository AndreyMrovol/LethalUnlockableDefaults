using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnlockOnStart;

namespace UnlockableDefaults
{
  [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
  public class Plugin : BaseUnityPlugin
  {
    internal static ManualLogSource logger;
    internal static Harmony harmony;

    private void Awake()
    {
      logger = Logger;

      harmony = new Harmony(PluginInfo.PLUGIN_GUID);
      harmony.PatchAll();

      ConfigManager.Init(Config);

      // Plugin startup logic
      Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
  }
}
