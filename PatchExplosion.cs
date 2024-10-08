using HarmonyLib;

namespace BoomerangFoo
{
    [HarmonyPatch(typeof(Explosion), nameof(Explosion.Init))]
    class ExplosionInitPatch
    {
        public static float radiusMultiplier = 1f;
        
        static float originalRadiusFinal;

        static void Prefix(Explosion __instance, bool isMiniExplosion)
        {
            originalRadiusFinal = __instance.radiusFinal;
            __instance.radiusFinal *= radiusMultiplier;
        }

        static void Postfix(Explosion __instance)
        {
            __instance.radiusFinal = originalRadiusFinal;
        }
    }
}
