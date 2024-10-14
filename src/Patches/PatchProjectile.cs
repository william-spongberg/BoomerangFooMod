using BoomerangFoo.GameModes;
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
            float boomerangSize = GameMode.selected.gameSettings.BoomerangSize;
            if (boomerangSize != 0f)
            {
                projectileColliderRadius = boomerangSize;
                __instance.transform.localScale = new Vector3(boomerangSize, boomerangSize, boomerangSize);
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
            float bouncinessMultiplier = movement != Movement.Returning && movement != Movement.Controlled ? GameMode.selected.gameSettings.BoomerangBouncinessMultiplier : 1f;
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
