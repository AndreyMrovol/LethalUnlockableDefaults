using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace UnlockOnStart
{
  [HarmonyPatch(typeof(ShipBuildModeManager))]
  internal class PlaceObjectPatch
  {
    [HarmonyPatch("PlaceShipObject")]
    [HarmonyPostfix]
    internal static void SaveToConfig(
      Vector3 placementPosition,
      Vector3 placementRotation,
      PlaceableShipObject placeableObject,
      bool placementSFX = true
    )
    {
      var unlockableID = placeableObject.unlockableID;
      UnlockableItem unlockableEntry = StartOfRound.Instance.unlockablesList.unlockables[unlockableID];
      string unlockableName = unlockableEntry.unlockableName;

      if (unlockableEntry.inStorage)
        return;

      ConfigManager.Add(unlockableName, "Position X", placementPosition.x);
      ConfigManager.Add(unlockableName, "Position Y", placementPosition.y);
      ConfigManager.Add(unlockableName, "Position Z", placementPosition.z);

      ConfigManager.Add(unlockableName, "Rotation X", placementRotation.x);
      ConfigManager.Add(unlockableName, "Rotation Y", placementRotation.y);
      ConfigManager.Add(unlockableName, "Rotation Z", placementRotation.z);
    }
  }
}
