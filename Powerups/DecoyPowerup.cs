using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomerangFoo.Powerups
{
    class DecoyPowerup
    {
        public static int MaxDecoyCount = 1;
        public static bool ReviveAsDecoy = false;

        public static void Register()
        {
            PatchPlayer.OnPreInit += PlayerInit;
            PatchPlayer.OnPostCreateDecoy += CreateDecoy;
        }

        public static void PlayerInit(Player player) 
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.maxDecoyCount = MaxDecoyCount;
            // TODO reviveAsDecoy
            playerState.reviveAsDecoy = ReviveAsDecoy;
            playerState.decoyCounter = 0;
        }

        public static void CreateDecoy(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.decoyCounter++;
            if (playerState.decoyCounter > playerState.maxDecoyCount) playerState.decoyCounter = playerState.maxDecoyCount;
            player.hasCreatedDecoy = playerState.decoyCounter >= playerState.maxDecoyCount;
        }

        public static void DecoyDie(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.decoyCounter--;
            if (playerState.decoyCounter < 0) playerState.decoyCounter = 0;
            player.hasCreatedDecoy = playerState.decoyCounter >= playerState.maxDecoyCount;
        }
    }
}
