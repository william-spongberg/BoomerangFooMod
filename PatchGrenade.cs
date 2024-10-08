using BoomerangFoo.Powerups;
using HarmonyLib;

namespace BoomerangFoo
{
    [HarmonyPatch(typeof(Grenade), "Explode")]
    class GrenadeExplodePatch
    {
        public static float explosionRadiusMultiplier = 1f;

        static void Prefix(Grenade __instance)
        {
            if (__instance.parentDisc == null) return;

            ExplosionInitPatch.radiusMultiplier = ExplosivePowerup.GetRadiusMultipler(__instance.parentDisc.discPowerup, __instance.parentDisc.isMiniDisc);
        }
    }
}
