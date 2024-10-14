using I2.Loc;
using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace BoomerangFoo.UI
{
    [HarmonyPatch(typeof(LocalizedString), nameof(LocalizedString.ToString))]
    class LocalizedStringToStringPatch
    {
        static void Postfix(LocalizedString __instance, ref string __result)
        {
            if (__instance.mTerm != null && __instance.mTerm.Length > 0 && (__result == null || __result.Length == 0))
            {
                // if an mTerm was present, but it failed to produce a localized string, then just return that mTerm
                __result = __instance.mTerm;
            }
        }
    }
}
