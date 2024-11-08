using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoomerangFoo.Patches;
using BoomerangFoo.UI;

namespace BoomerangFoo.Powerups
{
    class MultiBoomerangPowerup : CustomPowerup
    {
        public static int DefaultBoomerangsSplit = 5;

        private static MultiBoomerangPowerup instance;
        public static MultiBoomerangPowerup Instance
        {
            get
            {
                instance ??= new MultiBoomerangPowerup();
                return instance;
            }
        }

        public int BoomerangSplit { get; set; }

        protected MultiBoomerangPowerup()
        {
            Name = "Multiboomerang";
            Bitmask = PowerupType.MultiDisc;
            BoomerangSplit = DefaultBoomerangsSplit;
        }

        public override void Activate()
        {
            PatchPlayer.OnPreInit += PlayerInit;
        }

        public override void Deactivate()
        {
            PatchPlayer.OnPreInit -= PlayerInit;
        }

        private void PlayerInit(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.multiBoomerangSplit = BoomerangSplit;
        }

        public override void GenerateUI()
        {
            if (hasGeneratedUI) return;
            base.GenerateUI();
            var numBoomerangs = Modifiers.CloneModifierSetting($"customPowerup.{Name}.numBoomerangs", "Split Number", "Fall protection", $"customPowerup.{Name}.header");
            SettingIds.Add(numBoomerangs.id);

            string[] options = new string[19];
            string[] hints = new string[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = (i+2).ToString();
                hints[i] = $"Splits into {i+2} boomerangs";
            }
            numBoomerangs.SetSliderOptions(options, 3, hints);
            numBoomerangs.SetGameStartCallback((gameMode, sliderIndex) => {
                MultiBoomerangPowerup.Instance.BoomerangSplit = sliderIndex + 2;
            });
        }
    }
}
