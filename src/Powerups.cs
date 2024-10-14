using System;
using System.Collections.Generic;
using HarmonyLib;

namespace BoomerangFoo
{
    public class PowerupManager
    {
        public static Dictionary<List<PowerupType>, Player> powerupHistories = [];

        public static event Action<Player, List<PowerupType>, PowerupType> OnAcquirePowerup;
        public static void InvokeAcquirePowerup(Player player, List<PowerupType> powerupHistory, PowerupType newPowerup) 
        { 
            OnAcquirePowerup?.Invoke(player, powerupHistory, newPowerup); 
        }

        public static event Action<Player, List<PowerupType>, PowerupType> OnRemovePowerup;
        public static void InvokeRemovePowerup(Player player, List<PowerupType> powerupHistory, PowerupType newPowerup)
        {
            OnRemovePowerup?.Invoke(player, powerupHistory, newPowerup);
        }
    }

    [HarmonyPatch(typeof(List<PowerupType>), nameof(List<PowerupType>.Add))]
    class ListPowerupTypeAddPatch
    {

        static void Prefix(List<PowerupType> __instance, PowerupType item)
        {
            if (PowerupManager.powerupHistories.ContainsKey(__instance))
            {
                Player player = PowerupManager.powerupHistories[__instance];
                PowerupManager.InvokeAcquirePowerup(player, __instance, item);
            }
        }
    }

    [HarmonyPatch(typeof(List<PowerupType>), nameof(List<PowerupType>.Remove))]
    class ListPowerupTypeRemovePatch
    {

        static void Prefix(List<PowerupType> __instance, PowerupType item)
        {
            if (PowerupManager.powerupHistories.ContainsKey(__instance))
            {
                Player player = PowerupManager.powerupHistories[__instance];
                PowerupManager.InvokeRemovePowerup(player, __instance, item);
            }
        }
    }

    [HarmonyPatch(typeof(List<PowerupType>), nameof(List<PowerupType>.RemoveAt))]
    class ListPowerupTypeRemoveAtPatch
    {

        static void Prefix(List<PowerupType> __instance, int index)
        {
            if (PowerupManager.powerupHistories.ContainsKey(__instance))
            {
                Player player = PowerupManager.powerupHistories[__instance];
                PowerupManager.InvokeRemovePowerup(player, __instance, __instance[index]);
            }
        }
    }
}
