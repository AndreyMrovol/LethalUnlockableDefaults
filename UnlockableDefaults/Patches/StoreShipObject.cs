using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace UnlockOnStart
{
  [HarmonyPatch(typeof(ShipBuildModeManager))]
  internal class StoreObjectPatch
  {
    [HarmonyPatch("StoreShipObjectClientRpc")]
    [HarmonyPostfix]
    internal static void SaveToConfig(NetworkObjectReference objectRef, int playerWhoStored, int unlockableID)
    {
      UnlockableItem unlockableEntry = StartOfRound.Instance.unlockablesList.unlockables[unlockableID];
      string unlockableName = unlockableEntry.unlockableName;
      bool inStorage = unlockableEntry.inStorage;

      // ConfigManager.configFile.Bind(unlockableName, "In Storage", inStorage, "Whether the object is in storage or not");
    }
  }
}
