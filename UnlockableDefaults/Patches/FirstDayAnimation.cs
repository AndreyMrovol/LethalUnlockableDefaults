using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnlockableDefaults;

namespace UnlockOnStart
{
  [HarmonyPatch(typeof(StartOfRound))]
  internal class NewSavePatch
  {
    [HarmonyPatch("Start")]
    [HarmonyPriority(Priority.HigherThanNormal)]
    [HarmonyPrefix]
    internal static void Init()
    {
      Plugin.logger.LogDebug("NewSavePatch Init");

      List<UnlockableItem> unlockablesList = StartOfRound.Instance.unlockablesList.unlockables;

      foreach (UnlockableItem unlockable in unlockablesList)
      {
        var unlockableID = unlockablesList.IndexOf(unlockable);
        string unlockableName = unlockable.unlockableName;

        Plugin.logger.LogDebug($"Unlockable {unlockableName} / {unlockableID}");

        ConfigManager.InitialBind(unlockableName, "Position X");
        ConfigManager.InitialBind(unlockableName, "Position Y");
        ConfigManager.InitialBind(unlockableName, "Position Z");

        ConfigManager.InitialBind(unlockableName, "Rotation X");
        ConfigManager.InitialBind(unlockableName, "Rotation Y");
        ConfigManager.InitialBind(unlockableName, "Rotation Z");
      }
    }

    [HarmonyPatch("firstDayAnimation")]
    [HarmonyPriority(Priority.Last)]
    [HarmonyPostfix]
    internal static void LoadFromConfig()
    {
      List<UnlockableItem> unlockablesList = StartOfRound.Instance.unlockablesList.unlockables;

      // PlaceableShipObject[] objectsOfType = UnityEngine.Object.FindObjectsOfType<PlaceableShipObject>();
      GameObject ship = GameObject.Find("/Environment/HangarShip");

      foreach (UnlockableItem unlockable in unlockablesList)
      {
        var unlockableID = unlockablesList.IndexOf(unlockable);
        string unlockableName = unlockable.unlockableName;
        bool inStorage = unlockable.inStorage;
        bool isUnlocked = unlockable.alreadyUnlocked || unlockable.hasBeenUnlockedByPlayer;

        Plugin.logger.LogDebug($"Unlockable {unlockableName} / {unlockableID} : {inStorage} // {isUnlocked}");

        if (!isUnlocked)
          continue;

        if (unlockable.unlockableType == 0)
          continue;

        var posX = ConfigManager.TryGet(unlockableName, "Position X");
        var posY = ConfigManager.TryGet(unlockableName, "Position Y");
        var posZ = ConfigManager.TryGet(unlockableName, "Position Z");

        var rotX = ConfigManager.TryGet(unlockableName, "Rotation X");
        var rotY = ConfigManager.TryGet(unlockableName, "Rotation Y");
        var rotZ = ConfigManager.TryGet(unlockableName, "Rotation Z");

        Plugin.logger.LogDebug($"Unlockable {unlockableName} position: {posX}, {posY}, {posZ}");
        Plugin.logger.LogDebug($"Unlockable {unlockableName} rotation: {rotX}, {rotY}, {rotZ}");

        if (posX == 0f || posY == 0f || posZ == 0f)
          continue;

        if (inStorage)
          continue;

        PlaceableShipObject component = ship.GetComponentsInChildren<PlaceableShipObject>()
          .Where(x => x.unlockableID == unlockableID)
          .FirstOrDefault();

        Vector3 placementPosition = new Vector3(posX, posY, posZ);
        Vector3 placementRotation = new Vector3(rotX, rotY, rotZ);

        // Plugin.logger.LogDebug($"{gameObject.GetComponentInChildren<PlaceableShipObject>().unlockableID}");

        try
        {
          ShipBuildModeManager.Instance.PlaceShipObject(placementPosition, placementRotation, component, false);
        }
        catch (Exception e)
        {
          Plugin.logger.LogError($"Error placing {unlockableName} : {e}");
        }
      }
    }
  }
}
