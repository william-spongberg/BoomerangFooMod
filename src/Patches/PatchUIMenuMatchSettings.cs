using BoomerangFoo.GameModes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Reflection;
using BoomerangFoo.UI;

namespace BoomerangFoo.Patches
{
    public class PatchUIMenuMatchSettings
    {
        public static event Action<GameMode.Slot> OnMatchTypeSelected;
        public static void InvokeMatchTypeSelected(GameMode.Slot matchSlot) { OnMatchTypeSelected?.Invoke(matchSlot); }

    }


    [HarmonyPatch(typeof(UIMenuMatchSettings), nameof(UIMenuMatchSettings.UIResponse_DeathMatch))]
    class UIMenuMatchSettingsDeathMatchPatch
    {
        static void Prefix(UIMenuMatchSettings __instance)
        {
            PatchUIMenuMatchSettings.InvokeMatchTypeSelected(GameMode.Slot.Deathmatch);
            UIMenuMatchSettingsGoToNextSettingPatch.overrideMatch = true;
        }
    }

    [HarmonyPatch(typeof(UIMenuMatchSettings), nameof(UIMenuMatchSettings.UIResponse_TeamUp))]
    class UIMenuMatchSettingsTeamUpPatch
    {
        static void Prefix(UIMenuMatchSettings __instance)
        {
            PatchUIMenuMatchSettings.InvokeMatchTypeSelected(GameMode.Slot.TeamUp);
            UIMenuMatchSettingsGoToNextSettingPatch.overrideMatch = true;
        }
    }

    [HarmonyPatch(typeof(UIMenuMatchSettings), nameof(UIMenuMatchSettings.UIResponse_HideAndSeek))]
    class UIMenuMatchSettingsHideAndSeekPatch
    {
        static void Prefix(UIMenuMatchSettings __instance)
        {
            PatchUIMenuMatchSettings.InvokeMatchTypeSelected(GameMode.Slot.HideAndSeek);
            UIMenuMatchSettingsGoToNextSettingPatch.overrideMatch = true;
        }
    }

    [HarmonyPatch(typeof(UIMenuMatchSettings), nameof(UIMenuMatchSettings.UIResponse_GoldenBoomerang))]
    class UIMenuMatchSettingsGoldenBoomerangPatch
    {
        static void Prefix(UIMenuMatchSettings __instance)
        {
            PatchUIMenuMatchSettings.InvokeMatchTypeSelected(GameMode.Slot.GoldenBoomerang);
            UIMenuMatchSettingsGoToNextSettingPatch.overrideMatch = true;
        }
    }

    [HarmonyPatch(typeof(UIMenuMatchSettings), "GoToNextSetting")]
    class UIMenuMatchSettingsGoToNextSettingPatch
    {
        public static bool overrideMatch = false;

        static void Prefix(UIMenuMatchSettings __instance)
        {
            if (overrideMatch)
            {
                Singleton<SettingsManager>.Instance.teamMatch = GameMode.selected.teamMatch;
                Singleton<SettingsManager>.Instance.matchType = GameMode.selected.matchType;
                overrideMatch = false;
            }

        }
    }

    [HarmonyPatch(typeof(UIMenuMatchSettings), nameof(UIMenuMatchSettings.Init))]
    class UIMenuMatchSettingsInitPatch
    {
        private static MethodInfo goToNextSetting = typeof(Player).GetMethod("GoToNextSetting", BindingFlags.NonPublic | BindingFlags.Instance);
        private static bool hasInitialized = false;

        static void Postfix(UIMenuMatchSettings __instance)
        {
            var gamemodeModule = __instance.GetMatchSettingModule(UIMatchSettingModule.MatchSetting.GameMode);
            if (gamemodeModule != null && !hasInitialized)
            {
                var gamemodeChoices = gamemodeModule.gameObject.transform.GetChild(1).gameObject;

                for (int i = 1; i < GameMode.slots.Count; i++)
                {
                    // Create new gamemode button, copied from an existing one
                    var gamemode = GameMode.slots[i];
                    var template = gamemodeChoices.transform.GetChild(gamemode.slotTemplate);
                    var newChoice = UnityEngine.Object.Instantiate(template);
                    newChoice.transform.SetParent(gamemodeChoices.transform, false);

                    // Set the button text and set the onClick function
                    var newButton = newChoice.transform.GetChild(0).gameObject;
                    var buttonText = newChoice.GetComponentInChildren<TextMeshProUGUI>();
                    buttonText.text = gamemode.name;
                    var uiButton = newButton.GetComponent<UIButton>();
                    uiButton.onClick.RemoveAllListeners();
                    GameMode.Slot slot = (GameMode.Slot)i;
                    uiButton.onClick.AddListener(() =>
                    {
                        PatchUIMenuMatchSettings.InvokeMatchTypeSelected(slot);
                        goToNextSetting.Invoke(__instance, null);
                    });
                    var nav = uiButton.navigation;
                    nav.mode |= UnityEngine.UI.Navigation.Mode.Vertical;
                    uiButton.navigation = nav;
                    //newButton.transform.GetChild(0).gameObject.SetActive(false);

                    // Set the hint text and destroy the localization
                    var newHint = newChoice.transform.GetChild(1).gameObject;
                    var hintText = newHint.GetComponentInChildren<TextMeshProUGUI>();;
                    hintText.text = gamemode.hint;
                    var localize = newHint.GetComponentInChildren<I2.Loc.Localize>();
                    Component.Destroy(localize);
                }

                for (int i = 1; i < 4; i++)
                {
                    gamemodeChoices.transform.GetChild(i).gameObject.SetActive(false);
                }
                var first = gamemodeChoices.transform.GetChild(0).GetChild(0);
                var firstButton = first.GetComponent<UIButton>();
                var nav2 = firstButton.navigation;
                nav2.mode |= UnityEngine.UI.Navigation.Mode.Vertical;
                firstButton.navigation = nav2;


                if (GameMode.slots.Count > 6)
                {
                    var newGamemodeChoices = UnityEngine.Object.Instantiate(gamemodeChoices);
                    newGamemodeChoices.transform.SetParent(gamemodeModule.transform, false);
                    //newGamemodeChoices.transform.position += Vector3.down;
                    var newRectTransform = newGamemodeChoices.GetComponent<RectTransform>();
                    Vector2 currentPos = newRectTransform.anchoredPosition;
                    currentPos.y -= 150f;
                    newRectTransform.anchoredPosition = currentPos;
                    newRectTransform.localScale = new Vector3(0.75f, 0.75f, 1f);

                    var oldRectTransform = gamemodeChoices.GetComponent<RectTransform>();
                    currentPos = oldRectTransform.anchoredPosition;
                    currentPos.y += 80f;
                    oldRectTransform.anchoredPosition = currentPos;
                    oldRectTransform.localScale = new Vector3(0.75f, 0.75f, 1f);
                }
                hasInitialized = true;

                var modifierSettings = Modifiers.GetModifierSettings(__instance);
                Modifiers.settings = modifierSettings;
                foreach (GameMode gamemode in GameMode.registered.Values)
                {
                    gamemode.RegisterSettings();
                }
            }
        }
    }
}
