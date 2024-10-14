using BoomerangFoo.Patches;
using RewiredConsts;
using System.Linq;
using System.Reflection;
using UnityEngine.Profiling.Memory.Experimental;

namespace BoomerangFoo.GameModes
{
    public class PowerDrain : GameMode
    {
        private MethodInfo stopShield = typeof(Player).GetMethod("StopShield", BindingFlags.NonPublic | BindingFlags.Instance);

        public PowerDrain() : base("PowerDrain", "Power Drain", "Kill to steal powers", SettingsManager.MatchType.DeathMatch, false, 0)
        {
            
        }

        public override void Hook()
        {
            PatchPlayer.OnPostDie += OnDeath;
            PatchGameManager.OnPreAddPlayerKill += OnAddPlayerKill;
            PatchPlayer.OnPostGetReady += OnGetReady;
            PatchLevelManager.BlockPowerupSpawn = true;
        }

        public override void Unhook()
        {
            PatchPlayer.OnPostDie -= OnDeath;
            PatchGameManager.OnPreAddPlayerKill -= OnAddPlayerKill;
            PatchPlayer.OnPostGetReady -= OnGetReady;
            PatchLevelManager.BlockPowerupSpawn = false;
        }

        public void OnDeath(Player player)
        {
            if (_CustomSettings.PowerDrainLoseAll)
            {
                player.ClearPowerups();
            }
            else if (player.activePowerup != 0 && player.powerupHistory.Any())
            {
                player.activePowerup &= ~player.powerupHistory[0];
                player.powerupHistory.RemoveAt(0);
            }
        }

        public void OnAddPlayerKill(GameManager gameManager, Player killer, Player killed)
        {
            if (killer.IsShielded)
            {
                stopShield.Invoke(killer, null);
            }
            CollectPowerDrainPowerup(killer);
        }

        public void OnGetReady(Player player)
        {
            if (player.activePowerup == PowerupType.None)
            {
                player.StartShield();
            }
        }

        private void CollectPowerDrainPowerup(Player player)
        {
            if (player.alertBar != null && player.alertBar.currentAlert != 0)
            {
                player.alertBar.HideAlert();
            }
            PowerupType powerupType = Singleton<LevelManager>.Instance.SelectRandomPower(player);
            player.alertBar.ShowPowerup(powerupType);
            Singleton<ParticleManager>.Instance.PlayPFX(PFXType.SparkleBurst, player.transform.position, player.alertBar.GetPowerupColor(powerupType));
            Singleton<AudioManager>.Instance.PlayOneShotAttached("event:/powerups/collect_power", player.gameObject);
            player.StartPowerup(powerupType);
            player.playerInput.StartVibration(0.5f, 0.25f);
            player.metadata.PowerupsCollected++;
        }
    }
}