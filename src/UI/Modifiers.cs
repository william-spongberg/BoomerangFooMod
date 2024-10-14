using System;
using System.Collections.Generic;
using BoomerangFoo.GameModes;
using I2.Loc;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace BoomerangFoo.UI
{
    public class Modifiers
    {
        public static Dictionary<string, ModifierSetting> settings = new Dictionary<string, ModifierSetting>();

        public static GameObject GetModifiersSettingsContainer(UIMenuMatchSettings matchSettingsUI)
        {
            UIMatchSettingModule modifiers = matchSettingsUI.GetMatchSettingModule(UIMatchSettingModule.MatchSetting.Modifiers);
            GameObject settings = modifiers.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
            
            return settings;
        }

        public static Dictionary<string, ModifierSetting> GetModifierSettings(UIMenuMatchSettings matchSettingsUI)
        {
            Dictionary<string, ModifierSetting> settings = new Dictionary<string, ModifierSetting>();

            GameObject container = GetModifiersSettingsContainer(matchSettingsUI);
            for (int i = 0; i < container.transform.childCount; i++)
            {
                var setting = container.transform.GetChild(i);
                ModifierSetting modifierSetting;
                switch (setting.gameObject.name)
                {
                    case "ROW_HEAD":
                        var headTextMesh = setting.GetComponentInChildren<TextMeshProUGUI>();
                        Component.Destroy(setting.GetComponentInChildren<Localize>());
                        modifierSetting = new ModifierSetting(ModifierSetting.Type.Head, headTextMesh.text, headTextMesh.text, setting.gameObject, null);
                        break;
                    case "ROW":
                        var label = setting.GetChild(0);
                        var labelTextMesh = label.GetComponentInChildren<TextMeshProUGUI>();
                        var slider = setting.GetChild(1).GetComponent<UISliderButton>();
                        Component.Destroy(label.GetComponentInChildren<Localize>());
                        modifierSetting = new ModifierSetting(ModifierSetting.Type.Slider, labelTextMesh.text, labelTextMesh.text, setting.gameObject, slider);
                        break;
                    case "ROW_TOGGLES":
                        // the powerup cluster
                        var labelTemplate = settings["Match length"].gameObject.transform.GetChild(0);
                        var newLabel = UnityEngine.Object.Instantiate(labelTemplate.gameObject);
                        newLabel.transform.SetParent(setting.gameObject.transform, false);
                        newLabel.transform.SetSiblingIndex(0);
                        newLabel.SetActive(false);
                        modifierSetting = new ModifierSetting(ModifierSetting.Type.Powerup, "powerupSelections", "", setting.gameObject);
                        break;
                    default:
                        continue;
                }
                if (!settings.ContainsKey(modifierSetting.id))
                {
                    settings.Add(modifierSetting.id, modifierSetting);
                }
            }
            return settings;
        }

        public static ModifierSetting CloneModifierSetting(string id, string newLabel, string cloneSetting, string attachSetting)
        {
            if (!settings.ContainsKey(cloneSetting))
            {
                BoomerangFoo.Logger.LogError($"Could not clone modifier setting {cloneSetting}");
            }
            if (!settings.ContainsKey(attachSetting))
            {
                BoomerangFoo.Logger.LogError($"Could not attach to modifier setting {attachSetting}");
            }
            var cloneModifier = settings[cloneSetting];
            var settingContainer = cloneModifier.gameObject.transform.parent;
            var newSettingGO = UnityEngine.Object.Instantiate(cloneModifier.gameObject);
            newSettingGO.transform.SetParent(settingContainer, false);
            int attachIndex = settings[attachSetting].gameObject.transform.GetSiblingIndex();
            newSettingGO.transform.SetSiblingIndex(attachIndex + 1);
            var slider = newSettingGO.transform.GetChild(1).GetComponent<UISliderButton>();
            var labelTextMesh = newSettingGO.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            labelTextMesh.text = newLabel;
            Component.Destroy(newSettingGO.transform.GetChild(0).GetComponentInChildren<Localize>());

            var newSetting = new ModifierSetting(ModifierSetting.Type.Slider, id, newLabel, newSettingGO, slider);
            settings[newSetting.id] = newSetting;
            return newSetting;
        }
    }

    public class ModifierSetting
    {
        private readonly static FieldInfo helperText = typeof(UISliderButton).GetField("helperText", BindingFlags.NonPublic | BindingFlags.Instance);

        public enum Type
        {
            Head,
            Slider,
            Powerup
        }

        public Type type;
        public string id;
        public string text;
        public GameObject gameObject;
        public UISliderButton slider;

        public ModifierSetting(Type type, string id, string text, GameObject gameObject, UISliderButton slider)
        {
            this.type = type;
            this.id = id;
            this.text = text;
            this.gameObject = gameObject;
            this.slider = slider;
        }

        public ModifierSetting(Type type, string id, string text, GameObject gameObject)
        {
            this.type = type;
            this.id = id;
            this.text = text;
            this.gameObject = gameObject;
        }

        public void SetSliderOptions(string[] values, int defaultIndex, string[] hints)
        {
            if (type != Type.Slider) return;

            slider.SetMaxValue(values.Length - 1);
            slider.stringMap = values.Select(x => (LocalizedString)x).ToArray();
            slider.SetValue(defaultIndex);

            if (hints != null)
            {
                //UIHelperText hint = (UIHelperText)helperText.GetValue(slider);
                UIHelperText hint = slider.gameObject.GetComponent<UIHelperText>();
                if (hint != null)
                {
                    hint.strings = hints.Select(x => (LocalizedString)x).ToArray();
                }
            }
        }

        public void SetSliderCallback(UnityEngine.Events.UnityAction<int> onChange)
        {
            slider.OnValueChanged.RemoveAllListeners();
            slider.OnValueChanged.AddListener(onChange);
        }

        public void ActivatePowerupLabel()
        {
            // assumes label is index 0, padding is index 1
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        public void SetPowerupCallback(PowerupType defaultPowerups, Action<PowerupType> onChange)
        {
            var toggles = gameObject.transform.GetChild(2).transform.GetComponentsInChildren<UIPowerupToggle>();
            PowerupType powerupsActive = defaultPowerups;
            foreach (UIPowerupToggle toggle in toggles)
            {
                toggle.onValueChanged.RemoveAllListeners();
                toggle.isOn = (powerupsActive & toggle.powerupType) != 0;
                toggle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        powerupsActive |= toggle.powerupType;
                    } else
                    {
                        powerupsActive &= ~toggle.powerupType;
                    }
                    onChange(powerupsActive);
                });
                var nav = toggle.navigation;
                nav.mode = UnityEngine.UI.Navigation.Mode.Automatic;
                toggle.navigation = nav;
            }
        }
    }
}
