namespace BoomerangFoo.Powerups
{
    public class ExplosivePowerup
    {
        public static PowerupType PowerupBitmask = PowerupType.ExplosiveDisc;

        public static float RadiusMultiplier = 1f;
        public static float FreezingRadiusMultiplier = 1f;
        public static float MiniRadiusMultiplier = 1f;

        public static float GetRadiusMultipler(PowerupType discPowerup, bool isMiniDisc)
        {
            float multiplier = 1f;

            if (discPowerup.HasPowerup(PowerupType.SlimeDisc))
            {
                multiplier = _CustomSettings.ExplosiveFreezingRadiusMultiplier;
            }
            else if (discPowerup.HasPowerup(PowerupType.FireDisc))
            {
                // consider adding a different setting for fire
                multiplier = _CustomSettings.ExplosiveRadiusMultiplier;
            }
            else
            {
                multiplier = _CustomSettings.ExplosiveRadiusMultiplier;
            }
            if (isMiniDisc)
            {
                multiplier *= MiniRadiusMultiplier;
            }
            return multiplier;
        }
    }
}
