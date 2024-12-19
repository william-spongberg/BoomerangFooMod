using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace BoomerangFoo.Patches
{
    [HarmonyPatch(typeof(UIMenuMain), nameof(UIMenuMain.Init))]
    class UIMenuMainInitPatch
    {
        static string versionText = null;
        static void Postfix(UIMenuMain __instance)
        {
            var versionGo = GameObject.Find("Version Number");
            if (versionGo != null)
            {
                var text = versionGo.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    string version = text.text;
                    if (versionText == null)
                    {
                        versionText = $"{version} | MOD {MyPluginInfo.PLUGIN_VERSION}";
                    }
                    text.text = versionText;
                }
            }
        }
    }
}
