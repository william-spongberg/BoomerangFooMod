using BoomerangFoo.GameModes;
using BoomerangFoo.UI;
using HarmonyLib;
using System;

namespace BoomerangFoo.Patches
{
    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.PrepareMatchLength))]
    class SettingsManagerPrepareMatchLengthPatch
    {
        static bool Prefix(SettingsManager __instance)
        {
            if (GameMode.selected.gameSettings.MatchScoreLimit != 0)
            {
                __instance.matchScoreGoal = GameMode.selected.gameSettings.MatchScoreLimit;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.SelectMatchType))]
    class SettingsManagerSelectMatchTypePatch
    {
        public static Func<SettingsManager, SettingsManager.MatchType> GetMatchType;
        public static Func<SettingsManager, bool> GetIsTeamMatch;

        static void Postfix(SettingsManager __instance)
        {
            if (GetMatchType != null)
            {
                __instance.matchType = GetMatchType(__instance);
            }
            if (GetIsTeamMatch != null)
            {
                __instance.teamMatch = GetIsTeamMatch(__instance);
            }
            else if (__instance.matchType == SettingsManager.MatchType.GoldenDisc)
            {
                __instance.teamMatch = _CustomSettings.TeamGoldenBoomerang;
            }

        }
    }

    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.SaveModifiedMatchSettings))]
    class SettingsManagerSaveModifiedMatchSettingsPatch
    {
        static void Postfix()
        {
            Modifiers.SaveSettings();
        }
    }
}
