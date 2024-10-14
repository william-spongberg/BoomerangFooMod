using System;
using BoomerangFoo.GameModes;
using TMPro;
using UnityEngine;

namespace BoomerangFoo.UI
{
    public class GameModeUI
    {
        public static GameObject GetGameModeChoices(UIMenuMatchSettings matchSettingsUI, int row = 0)
        {
            UIMatchSettingModule gamemodeModule = matchSettingsUI.GetMatchSettingModule(UIMatchSettingModule.MatchSetting.GameMode);
            GameObject gamemodeChoices = gamemodeModule.gameObject.transform.GetChild(1 + row).gameObject;

            return gamemodeChoices;
        }

        public static GameObject CloneGameModeButtonTemplate(GameObject gamemodeChoices, GameMode.Slot slot)
        {
            var template = gamemodeChoices.transform.GetChild((int)slot);
            var newChoice = UnityEngine.Object.Instantiate(template);

            return newChoice.gameObject;
        }

        public static void ModifyGameModeChoice(GameObject gamemodeChoice, string text, string hint, UnityEngine.Events.UnityAction buttonAction)
        {
            var buttonGO = gamemodeChoice.transform.GetChild(0).gameObject;
            var buttonText = gamemodeChoice.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = text;
            var uiButton = buttonGO.GetComponent<UIButton>();
            uiButton.onClick.RemoveAllListeners();
            uiButton.onClick.AddListener(buttonAction);

            // Set the hint text and destroy the localization
            var newHint = gamemodeChoice.transform.GetChild(1).gameObject;
            var hintText = newHint.GetComponentInChildren<TextMeshProUGUI>(); ;
            hintText.text = hint;
            var localize = newHint.GetComponentInChildren<I2.Loc.Localize>();
            Component.Destroy(localize);
        }
    }
}
