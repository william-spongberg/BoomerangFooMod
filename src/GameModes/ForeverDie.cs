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

        public ForeverDie() : base("ForeverDie", "Slaughter House", "Unlimited respawns", SettingsManager.MatchType.DeathMatch, false, 0)
        {
            
        }

        public override void Hook()
        {
            PatchPlayer.OnPostDie += OnDeath;
            PatchGameManager.OnPreAddPlayerKill += OnAddPlayerKill;
        }

        public override void Unhook()
        {
            PatchPlayer.OnPostDie -= OnDeath;
            PatchGameManager.OnPreAddPlayerKill -= OnAddPlayerKill;
        }

        public override void RegisterSettings()
        {
            // string headerId = $"gameMode.{id}.header";
            // var header = Modifiers.CloneModifierSetting(headerId, name, "ui_boomerangs", "ui_label_friendlyfire");
        }

        public void OnDeath(Player player)
        {

        }

        public void OnAddPlayerKill(GameManager gameManager, Player killer, Player killed) {
            // debug logging
            BoomerangFoo.Logger.LogInfo($"Player {killer.playerID} killed player {killed.playerID}!");
            BoomerangFoo.Logger.LogInfo($"Player {killer.playerID} has {killer.killsThisRound} kills this round!");
            
            // respawn killed player
            gameManager.RespawnPlayer(killed);
        }
    }
}