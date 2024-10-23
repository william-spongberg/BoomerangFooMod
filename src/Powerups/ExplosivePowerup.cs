using BoomerangFoo.UI;

namespace BoomerangFoo.Powerups
{
    public class ExplosivePowerup : CustomPowerup
    {
        public static PowerupType PowerupBitmask = PowerupType.ExplosiveDisc;

        public static float DefaultRadiusMultiplier = 1f;
        public static float DefaultFreezingRadiusMultiplier = 1f;
        public static float DefaultMiniRadiusMultiplier = 1f;

        private static ExplosivePowerup instance;
        public static ExplosivePowerup Instance { get
            {
                instance ??= new ExplosivePowerup();
                return instance;
            } 
        }

        public float RadiusMultiplier { get; set; }
        public float FreezingRadiusMultiplier { get; set; }
        public float MiniRadiusMultiplier { get; set; }

        protected ExplosivePowerup()
        {
            Name = "Explosive";
            Bitmask = PowerupType.ExplosiveDisc;
            RadiusMultiplier = DefaultRadiusMultiplier;
            FreezingRadiusMultiplier = DefaultFreezingRadiusMultiplier;
            MiniRadiusMultiplier = DefaultMiniRadiusMultiplier;
        }

        public override void GenerateUI()
        {
            if (hasGeneratedUI) return;
            base.GenerateUI();

            // defaultRadius
            var defaultRadius = Modifiers.CloneModifierSetting($"customPowerup.{Name}.defaultRadius", "Radius Factor", "Fall protection", $"customPowerup.{Name}.header");
            SettingIds.Add(defaultRadius.id);

            float[] radiusValues = [0.25f, 0.5f, 0.75f, 1f, 1.25f, 1.5f, 2f, 2.5f, 3f, 20f];
            string[] radiusOptions = new string[radiusValues.Length];
            string[] radiusHints = new string[radiusOptions.Length];
            for (int i = 0; i < radiusValues.Length; i++)
            {
                radiusOptions[i] = radiusValues[i].ToString();
                radiusHints[i] = $"Multiply explosion radius by {radiusOptions[i]}x";
            }
            radiusHints[3] = "Normal explosion radius";
            radiusOptions[radiusOptions.Length - 1] = "NUKE";
            radiusHints[radiusHints.Length - 1] = "nuke.";
            defaultRadius.SetSliderOptions(radiusOptions, 3, radiusHints);
            defaultRadius.SetSliderCallback((sliderIndex) => {
                ExplosivePowerup.Instance.RadiusMultiplier = radiusValues[sliderIndex];
                BoomerangFoo.Logger.LogInfo($"Explosion radius {ExplosivePowerup.Instance.RadiusMultiplier}");
            });
        }

        public static float GetRadiusMultipler(PowerupType discPowerup, bool isMiniDisc)
        {
            if (!Instance.IsActivated) return 1;

            float multiplier = 1f;

            if (discPowerup.HasPowerup(PowerupType.SlimeDisc))
            {
                multiplier = Instance.FreezingRadiusMultiplier;
            }
            else if (discPowerup.HasPowerup(PowerupType.FireDisc))
            {
                // consider adding a different setting for fire
                multiplier = Instance.RadiusMultiplier;
            }
            else
            {
                multiplier = Instance.RadiusMultiplier;
            }
            if (isMiniDisc)
            {
                multiplier *= Instance.RadiusMultiplier;
            }
            return multiplier;
        }
    }
}
