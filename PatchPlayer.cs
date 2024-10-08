using BoomerangFoo.Powerups;
using HarmonyLib;
using System;
using System.Reflection;

namespace BoomerangFoo
{
    class PatchPlayer
    {
        public static event Action<Player> OnPostGetReady;
        public static void InvokePostGetReady(Player player) { OnPostGetReady?.Invoke(player); }

        public static event Action<Player> OnPostSpawnIn;
        public static void InvokePostSpawnIn(Player player) { OnPostSpawnIn?.Invoke(player); }

        public static event Action<Player> OnPreInit;
        public static void InvokePreInit(Player player) { OnPreInit?.Invoke(player); }

        public static event Action<Player> OnPreUpdate;
        public static void InvokePreUpdate(Player player) { OnPreUpdate?.Invoke(player); }

        public static event Action<Player> OnPostUpdate;
        public static void InvokePostUpdate(Player player) { OnPostUpdate?.Invoke(player); }

        public static event Action<Player> OnPreStartShield;
        public static void InvokePreStartShield(Player player) { OnPreStartShield?.Invoke(player); }

        public static event Action<Player> OnPostCreateDecoy;
        public static void InvokePostCreateDecoy(Player player) { OnPostCreateDecoy?.Invoke(player); }

        public static event Action<Player> OnPostRunGoldenDiscTimer;
        public static void InvokePostRunGoldenDiscTimer(Player player) { OnPostRunGoldenDiscTimer?.Invoke(player); }

        public static event Action<Player> OnPreDie;
        public static void InvokePreDie(Player player) { OnPreDie?.Invoke(player); }

        public static event Action<Player> OnPostDie;
        public static void InvokePostDie(Player player) { OnPostDie?.Invoke(player); }

        public static Func<Player, bool> PreStartFall;

        public static Predicate<Player> DoToggleGoldenDiscPFX;

        public static event Action<Player, PowerupType> OnPreStartPowerup;
        public static void InvokePreStartPowerup(Player player, PowerupType powerupType) { OnPreStartPowerup?.Invoke(player, powerupType); }

        public static event Action<Player, PowerupType> OnPostStartPowerup;
        public static void InvokePostStartPowerup(Player player, PowerupType powerupType) { OnPostStartPowerup?.Invoke(player, powerupType); }

        public static event Action<Player> OnPreStartFall;
        public static void InvokePreStartFall(Player player) { OnPreStartFall?.Invoke(player); }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.GetReady))]
    class PlayerGetReadyPatch
    {
        static void Postfix(Player __instance)
        {
            PatchPlayer.InvokePostGetReady(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), "SpawnIn")]
    class PlayerSpawnInPatch
    {
        static void Postfix(Player __instance)
        {
            PatchPlayer.InvokePostSpawnIn(__instance);
            if (_CustomSettings.StartupPowerUps != 0 && Singleton<GameManager>.Instance.roundNumber == 1)
            {
                CommonFunctions.GetEnumPowerUpValues(_CustomSettings.StartupPowerUps).ForEach(delegate (PowerupType i)
                {
                    __instance.StartPowerup(i);
                });
            }
        }
    }

    [HarmonyPatch(typeof(Player), "Start")]
    class PlayerStartPatch
    {

        static void Postfix(Player __instance)
        {
            __instance.gameObject.AddComponent<PlayerState>();
        }
    }

    [HarmonyPatch(typeof(Player), "Init")]
    class PlayerInitPatch
    {
        static readonly FieldInfo activePowerups = typeof(Player).GetField("maxActivePowerups", BindingFlags.NonPublic | BindingFlags.Instance);

        static void Prefix(Player __instance)
        {
            PatchPlayer.InvokePreInit(__instance);
            PowerupManager.powerupHistories[__instance.powerupHistory] = __instance;
            activePowerups.SetValue(__instance, _CustomSettings.MaxPowerUps);
        }
    }

    [HarmonyPatch(typeof(Player), "Update")]
    class PlayerUpdatePatch
    {
        static void Prefix(Player __instance)
        {
            PatchPlayer.InvokePreUpdate(__instance);
        }

        static void Postfix(Player __instance)
        {
            PatchPlayer.InvokePostUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), "StartReverseInputsTimer")]
    class PlayerStartReverseInputsTimerPatch
    {
        static void Prefix(Player __instance, ref float duration)
        {
            duration = BamboozlePowerup.StartReverseInputs(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), "RunReverseInputsTimer")]
    class PlayerRunReverseInputsTimerPatch
    {
        static void Prefix(Player __instance)
        {
            BamboozlePowerup.StopReverseInputs(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.StartShield))]
    class PlayerStartShieldPatch
    {
        static void Prefix(Player __instance)
        {
            PatchPlayer.InvokePreStartShield(__instance);
        }

        static void Postfix(Player __instance)
        {
            BoomerangFoo.Logger.LogInfo("Shield triggered1");
            __instance.shieldHitsLeft = CommonFunctions.GetPlayerState(__instance)?.shieldHits ?? 1;
            BoomerangFoo.Logger.LogInfo($"Got shield with {__instance.shieldHitsLeft} hits");
        }
    }

    [HarmonyPatch(typeof(Player), "StartShieldHitCounter")]
    class PlayerStartStartShieldHitCounterPatch
    {
        static void Postfix(Player __instance)
        {
            BoomerangFoo.Logger.LogInfo("Shield triggered");
            __instance.shieldHitsLeft = CommonFunctions.GetPlayerState(__instance)?.shieldHits ?? 1;
        }
    }

    [HarmonyPatch(typeof(Player), "CreateDecoy")]
    class PlayerCreateDecoyPatch
    {
        static void Postfix(Player __instance)
        {
            PatchPlayer.InvokePostCreateDecoy(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), "RunGoldenDiscTimer")]
    class PlayerRunGoldenDiscTimerPatch
    {
        static void Postfix(Player __instance)
        {
            PatchPlayer.InvokePostRunGoldenDiscTimer(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.Die))]
    class PlayerDiePatch
    {
        static void Prefix( Player __instance)
        {
            GameManagerAddPlayerKillPatch.killedPlayer = __instance;
            PatchPlayer.InvokePreDie(__instance);
        }

        static void Postfix(Player __instance)
        {
            PatchPlayer.InvokePostDie(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.StartFall))]
    class PlayerStartFallPatch
    {
        static bool Prefix(Player __instance)
        {
            if (PatchPlayer.PreStartFall != null)
            {
                bool doOriginal = PatchPlayer.PreStartFall.Invoke(__instance);
                return doOriginal;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Player), "ToggleGoldenDiscPFX")]
    class PlayerToggleGoldenDiscPFXPatch
    {
        static bool Prefix(Player __instance)
        {
            if (PatchPlayer.DoToggleGoldenDiscPFX == null) return true;
            return PatchPlayer.DoToggleGoldenDiscPFX.Invoke(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.StartPowerup))]
    class PlayerStartPowerupPatch
    {
        static void Prefix(Player __instance, PowerupType newPowerup)
        {
            PatchPlayer.InvokePreStartPowerup(__instance, newPowerup);
        }

        static void Postfix(Player __instance, PowerupType newPowerup)
        {
            PatchPlayer.InvokePostStartPowerup(__instance, newPowerup);
        }
    }
}
