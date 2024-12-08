using BoomerangFoo.UI;
using System.Collections.Generic;
using System.Reflection;

namespace BoomerangFoo.Powerups
{
    public class FirePowerup : CustomPowerup
    {
        private static readonly FieldInfo playerBurnDuration = typeof(Player).GetField("burnDuration", BindingFlags.NonPublic | BindingFlags.Instance);
        private const float originalBurnDuration = 2.5f;

        private static FirePowerup instance;
        public static FirePowerup Instance { get
            {
                instance ??= new FirePowerup();
                return instance;
            } 
        }

        public float BurnDuration { get; set; }


        protected FirePowerup()
        {
            Name = "Fire";
            Bitmask = PowerupType.FireDisc;
            BurnDuration = originalBurnDuration;
        }

        public override void Activate()
        {
            base.Activate();
            PowerupManager.OnAcquirePowerup += AcquirePowerup;
            PowerupManager.OnRemovePowerup += RemovePowerup;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            PowerupManager.OnAcquirePowerup -= AcquirePowerup;
            PowerupManager.OnRemovePowerup -= RemovePowerup;
        }

        public override void GenerateUI()
        {
            if (hasGeneratedUI) return;
            base.GenerateUI();

            // defaultRadius
            var fireDuration = Modifiers.CloneModifierSetting($"customPowerup.{Name}.defaultRadius", "Burning Survival", "ui_label_edgeprotection", $"customPowerup.{Name}.header");
            SettingIds.Add(fireDuration.id);

            float[] burnValues = [0.2f, 1.25f, 2.5f, 4f, 8f, 20f];
            string[] options = ["Instant", "Short", "Default", "Long", "Longer", "Human Torch"];
            string[] hints = new string[options.Length];
            for (int i = 1; i < burnValues.Length; i++)
            {
                hints[i] = $"Player with fire power survives burning for {burnValues[i]} seconds";
            }
            hints[0] = "Player with fire power is a glass cannon";
            fireDuration.SetSliderOptions(options, 2, hints);
            fireDuration.SetGameStartCallback((gameMode, sliderIndex) => {
                FirePowerup.Instance.BurnDuration = burnValues[sliderIndex];
            });
        }

        public void AcquirePowerup(Player player, List<PowerupType> powerupHistory, PowerupType newPowerup)
        {
            if (!newPowerup.HasPowerup(Bitmask)) return;

            playerBurnDuration.SetValue(player, BurnDuration);
        }

        public void RemovePowerup(Player player, List<PowerupType> powerupHistory, PowerupType newPowerup)
        {
            if (!newPowerup.HasPowerup(Bitmask)) return;

            playerBurnDuration.SetValue(player, originalBurnDuration);
        }
    }
}
