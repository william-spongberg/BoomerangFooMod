using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoomerangFoo.Patches;
using BoomerangFoo.UI;

namespace BoomerangFoo.Powerups
{
    class DecoyPowerup : CustomPowerup
    {
        public static int DefaultMaxDecoyCount = 1;
        public static bool DefaultReviveAsDecoy = false;

        private static DecoyPowerup instance;
        public static DecoyPowerup Instance { get
            {
                instance ??= new DecoyPowerup();
                return instance;
            } 
        }

        public int MaxDecoyCount;
        public bool ReviveAsDecoy;

        protected DecoyPowerup()
        {
            Name = "Decoy";
            Bitmask = PowerupType.Decoy;
            MaxDecoyCount = DefaultMaxDecoyCount;
            ReviveAsDecoy = DefaultReviveAsDecoy;
        }

        public override void Activate()
        {
            base.Activate();
            PatchPlayer.OnPreInit += PlayerInit;
            PatchPlayer.OnPostCreateDecoy += CreateDecoy;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            PatchPlayer.OnPreInit -= PlayerInit;
            PatchPlayer.OnPostCreateDecoy -= CreateDecoy;
        }

        public override void GenerateUI()
        {
            if (hasGeneratedUI) return;
            base.GenerateUI();
            var maxDecoys = Modifiers.CloneModifierSetting($"customPowerup.{Name}.maxDecoys", "Maximum Decoys", "Fall protection", $"customPowerup.{Name}.header");
            SettingIds.Add(maxDecoys.id);

            string[] options = new string[31];
            string[] hints = new string[31];
            options[0] = "Infinite";
            hints[0] = "I am not responsible for what happens...";
            for (int i = 1; i < 31; i++)
            {
                options[i] = i.ToString();
                hints[i] = $"Can spawn up to {i} decoys.";
            }
            maxDecoys.SetSliderOptions(options, 1, hints);
            maxDecoys.SetSliderCallback((sliderIndex) => {
                // maxValue / 2 is big and no chance of overflow
                int maxValue = sliderIndex == 0 ? int.MaxValue / 2 : sliderIndex;
                DecoyPowerup.Instance.MaxDecoyCount = maxValue;
            });

            // revive (TODO)
            //var revive = Modifiers.CloneModifierSetting($"customPowerup.{Name}.respawn", "Revive As Decoy", "Warm up round", $"customPowerup.{Name}.maxDecoys");
            //SettingIds.Add(revive.id);
            //revive.SetSliderOptions(["Off", "On"], 0, ["You die as normal", "You can take the place of your decoy"]);
            //revive.SetSliderCallback((sliderIndex) => {
            //    DecoyPowerup.Instance.ReviveAsDecoy = (sliderIndex == 1);
            //});
        }

        private void PlayerInit(Player player) 
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.maxDecoyCount = MaxDecoyCount;
            // TODO reviveAsDecoy
            playerState.reviveAsDecoy = ReviveAsDecoy;
            playerState.decoyCounter = 0;
        }

        private void CreateDecoy(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.decoyCounter++;
            if (playerState.decoyCounter > playerState.maxDecoyCount) playerState.decoyCounter = playerState.maxDecoyCount;
            player.hasCreatedDecoy = playerState.decoyCounter >= playerState.maxDecoyCount;
        }

        public void DecoyDie(Player player)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(player);
            playerState.decoyCounter--;
            if (playerState.decoyCounter < 0) playerState.decoyCounter = 0;
            player.hasCreatedDecoy = playerState.decoyCounter >= playerState.maxDecoyCount;
        }
    }
}
