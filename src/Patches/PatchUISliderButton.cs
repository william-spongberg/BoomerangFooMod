using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine.UI;

namespace BoomerangFoo.Patches
{
    [HarmonyPatch(typeof(UISliderButton), "UpdateVisuals")]
    class UISliderButtonUpdateVisualsPatch
    {
        static void Postfix(UISliderButton __instance)
        {
            if (__instance?.text?.text == null) return;

            string text = __instance.text.text;
            if (text.EndsWith(" missing"))
            {
                // Remove " missing" that gets appended to "Impossible" due to missing translation
                __instance.text.text = text.Remove(text.Length - " missing".Length);
            }
        }
    }
}
