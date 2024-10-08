using HarmonyLib;
using UnityEngine;
using static Projectile;

namespace BoomerangFoo.Patches
{
    [HarmonyPatch(typeof(Projectile), "Update")]
    class ProjectileUpdatePatch
    {
        static void Prefix(Projectile __instance)
        {
            if (_CustomSettings.BoomerangSize != 0f)
            {
                projectileColliderRadius = _CustomSettings.BoomerangSize;
                __instance.transform.localScale = new Vector3(_CustomSettings.BoomerangSize, _CustomSettings.BoomerangSize, _CustomSettings.BoomerangSize);
            }
        }
    }

    [HarmonyPatch(typeof(Projectile), nameof(Projectile.Bounce))]
    class ProjectileBouncePatch
    {
        static float bounciness;

        static void Prefix(Projectile __instance)
        {
            Movement movement = __instance.movement;
            float bouncinessMultiplier = movement != Movement.Returning && movement != Movement.Controlled ? _CustomSettings.BoomerangBouncinessMultiplier : 1f;
            if (_CustomSettings.BoomerangBouncinessMultiplierTiedToPowerUp != 0
                && CommonFunctions.GetActorAsPlayer(__instance.owner) != null
                && (CommonFunctions.GetActorAsPlayer(__instance.owner).activePowerup & _CustomSettings.BoomerangBouncinessMultiplierTiedToPowerUp) == 0)
            {
                // we can use a bitmask to make only some powerups have this bouncy property
                bouncinessMultiplier = 1f;
            }
            bounciness = __instance.bounciness * bouncinessMultiplier;
            __instance.bounciness = bounciness;
        }

        static void Postfix(Projectile __instance)
        {
            __instance.bounciness = bounciness;
        }
    }
}
