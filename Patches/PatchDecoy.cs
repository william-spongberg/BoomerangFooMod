using BoomerangFoo.Powerups;
using HarmonyLib;

namespace BoomerangFoo.Patches
{
    [HarmonyPatch(typeof(Decoy), nameof(Decoy.Die))]
    class DecoyDiePatch
    {
        static void Prefix(Decoy __instance)
        {
            if (__instance.owner != null)
            {
                DecoyPowerup.DecoyDie(__instance.owner);
            }
        }
    }
}
