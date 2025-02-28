using BoomerangFoo.Patches;
using BoomerangFoo.UI;
using RewiredConsts;
using System.Linq;
using System.Reflection;
using UnityEngine.Profiling.Memory.Experimental;

namespace BoomerangFoo.GameModes
{
    public class ForeverDie : GameMode
    {

        public ForeverDie() : base("ForeverDie", "Slaughter House", "Kill until you win", SettingsManager.MatchType.GoldenDisc, false, 0)
        {
            
        }

        public override void Hook()
        {
            PatchPlayer.OnPostDie += OnDeath;
        }

        public override void Unhook()
        {
            PatchPlayer.OnPostDie -= OnDeath;
        }

        public override void RegisterSettings()
        {
            // string headerId = $"gameMode.{id}.header";
            // var header = Modifiers.CloneModifierSetting(headerId, name, "ui_boomerangs", "ui_label_friendlyfire");


        }

        public void OnDeath(Player player)
        {
            BoomerangFoo.Logger.LogInfo($"Player {player.playerID} died!");
            player.Init(1, false, true, true);
        }
    }
}