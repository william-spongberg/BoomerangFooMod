using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BoomerangFoo.Powerups
{
    public class BamboozlePowerup
    {
        public static bool Immunity = true;
        public static float Duration = 15f;

        static readonly FieldInfo isPowerupTimerOn = typeof(Player).GetField("isPowerupTimerOn", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo powerupTimer = typeof(Player).GetField("powerupTimer", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Register()
        {
            PatchPlayer.OnPreInit += PlayerInit;
        }

        public static void PlayerInit(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.reverseInputsDuration = Duration;
            playerState.reverseInputsImmunity = Immunity;
        }

        public static float StartReverseInputs(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);

            BoomerangFoo.Logger.LogInfo($"Reverse immunity {playerState.reverseInputsImmunity}, {player.IsInvulnerable}");
            if (playerState.reverseInputsImmunity)
            {
                player.IsInvulnerable = true;
            }

            float duration = playerState.reverseInputsDuration;
            BoomerangFoo.Logger.LogInfo($"Reverse immunity {playerState.reverseInputsImmunity}, {player.IsInvulnerable}");
            return duration;
        }

        public static void StopReverseInputs(Player player)
        {
            if ((bool)isPowerupTimerOn.GetValue(player))
            {
                if ((float)powerupTimer.GetValue(player) <= 0)
                {
                    player.IsInvulnerable = false;
                }
            }
        }
    }
}
