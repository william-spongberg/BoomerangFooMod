using BoomerangFoo.Patches;
using RewiredConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BoomerangFoo.Powerups
{
    public class FlyingPowerup
    {
        public static PowerupType PowerupBitmask = _CustomSettings.FlyingPowerUp;
        public static float Duration = _CustomSettings.FlyingDuration;

        static readonly FieldInfo reviveTimeUIClock = typeof(Player).GetField("reviveTimeUIClock", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo reviveTimeUI = typeof(Player).GetField("reviveTimeUI", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo isFalling = typeof(Player).GetField("isFalling", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Register()
        {
            PatchPlayer.OnPreInit += PlayerInit;
            PatchPlayer.OnPostSpawnIn += PostSpawnIn;
            PatchPlayer.OnPreUpdate += PreUpdate;
            PatchPlayer.PreStartFall = PreStartFall;
        }

        public static void PlayerInit(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.flyingForceFall = false;
            playerState.flyingTimer = 0f;
            playerState.isFlyingTimerOn = false;
            playerState.isFlying = false;
        }

        public static void PostSpawnIn(Player player)
        {
            //refreshFlyingMethod.Invoke(player, null);
            RefreshFlying(player);
        }

        public static void PreUpdate(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            RunFlyingTimer(player);
        }

        public static bool PreStartFall(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);

            if (PowerupBitmask != 0 && player.activePowerup.HasPowerup(PowerupBitmask) && !playerState.flyingForceFall)
            {
                StartFlyingTimer(player, playerState.flyingDuration);
                return false;
            }
            return true;
        }

        public static void StartFlyingTimer(Player player, float duration)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);

            if (!playerState.isFlyingTimerOn)
            {
                ((Image)(reviveTimeUIClock.GetValue(player))).enabled = true;

                if (playerState.resetOnGround)
                {
                    playerState.flyingTimeLimit = duration;
                    playerState.flyingTimer = duration;
                }
                playerState.isFlyingTimerOn = true;
                playerState.isFlying = true;
                ((Canvas)(reviveTimeUI.GetValue(player))).GetComponent<Animator>().SetBool("IsVisible", value: true);
            }
        }

        private static void RunFlyingTimer(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            var reviveClock = ((Image)(reviveTimeUIClock.GetValue(player)));
            var reviveUI = ((Canvas)(reviveTimeUI.GetValue(player)));

            if (playerState.isFlying && !player.IsPositionOverHazard(player.transform.position))
            {
                playerState.isFlyingTimerOn = false;
                playerState.isFlying = false;
                reviveUI.GetComponent<Animator>().SetBool("IsVisible", value: false);
            }

            if (playerState.isFlyingTimerOn)
            {
                if (playerState.flyingTimer > 0f)
                {
                    reviveClock.fillAmount = playerState.flyingTimer / playerState.flyingTimeLimit;
                    playerState.flyingTimer -= Time.deltaTime * player.timeScaler;
                    return;
                }
                reviveUI.GetComponent<Animator>().SetBool("IsVisible", value: false);
                reviveClock.fillAmount = 0f;
                playerState.flyingTimer = playerState.flyingDuration;
                playerState.isFlyingTimerOn = false;
                playerState.flyingForceFall = true;
                playerState.isFlying = false;
            }
            else if (playerState.isFlying)
            {
                reviveUI.GetComponent<Animator>().SetBool("IsVisible", value: false);
                reviveClock.fillAmount = 0f;
                playerState.flyingTimer = playerState.flyingDuration;
                playerState.isFlyingTimerOn = false;
            }
        }

        public static void RefreshFlying(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);

            playerState.flyingTimeLimit = playerState.flyingDuration;
            playerState.flyingTimer = playerState.flyingDuration;
            playerState.isFlyingTimerOn = false;
            playerState.flyingForceFall = false;
            playerState.isFlying = false;

            var reviveClock = ((Image)(reviveTimeUIClock.GetValue(player)));
            var reviveUI = ((Canvas)(reviveTimeUI.GetValue(player)));
            reviveUI.GetComponent<Animator>().SetBool("IsVisible", value: false);
            reviveClock.fillAmount = 0f;
        }
    }
}
