using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomerangFoo.Powerups
{
    class ShieldPowerup
    {
        public static int ShieldHits = 1;

        public static void Register()
        {
            PatchPlayer.OnPreInit += PlayerInit;
        }

        public static void PlayerInit(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.shieldHits = ShieldHits;
        }
    }
}
