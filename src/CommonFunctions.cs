using System.Collections.Generic;
using System;
using System.Linq;

namespace BoomerangFoo
{
    public static class CommonFunctions
    {
        public static List<PowerupType> GetEnumPowerUpValues(PowerupType powerups)
        {
            return (from PowerupType ps in Enum.GetValues(typeof(PowerupType))
                    where powerups.HasFlag(ps)
                    select ps).ToList();
        }

        public static Player GetActorAsPlayer(Actor actor)
        {
            return (actor is Player) ? (actor as Player) : null;
        }

        public static PlayerState GetPlayerState(Player player)
        {
            return player.GetComponent<PlayerState>();
        }

        public static PowerupType PreviousActive(List<PowerupType> powerupHistory)
        {
            PowerupType previous = PowerupType.None;
            for (int i = 0; i < powerupHistory.Count - 1; i++)
            {
                previous |= powerupHistory[i];
            }
            return previous;
        }
    }
}