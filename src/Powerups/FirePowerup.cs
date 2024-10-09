using System.Collections.Generic;
using System.Reflection;

namespace BoomerangFoo.Powerups
{
    public class FirePowerup
    {
        public static PowerupType PowerupBitmask = PowerupType.FireDisc;

        public static float BurnDuration = originalBurnDuration;

        public static readonly FieldInfo playerBurnDuration = typeof(Player).GetField("burnDuration", BindingFlags.NonPublic | BindingFlags.Instance);
        public const float originalBurnDuration = 2.5f;

        public static void Register()
        {
            PowerupManager.OnAcquirePowerup += AcquirePowerup;
            PowerupManager.OnRemovePowerup += RemovePowerup;
        }

        public static void AcquirePowerup(Player player, List<PowerupType> powerupHistory, PowerupType newPowerup)
        {
            if (!newPowerup.HasPowerup(PowerupBitmask)) return;

            playerBurnDuration.SetValue(player, BurnDuration);
        }

        public static void RemovePowerup(Player player, List<PowerupType> powerupHistory, PowerupType newPowerup)
        {
            if (!newPowerup.HasPowerup(PowerupBitmask)) return;

            playerBurnDuration.SetValue(player, originalBurnDuration);
        }
    }
}
