using BoomerangFoo.Patches;
using BoomerangFoo.UI;
using HarmonyLib;
using RewiredConsts;
using System.Linq;
using System.Reflection;
using UnityEngine.Profiling.Memory.Experimental;

namespace BoomerangFoo.GameModes
{
    public class SlaughterHouse : GameMode
    {

        public int PlayerLives = 1;
        public float RespawnDelay = 1f;

        private int[] PlayerLivesArray = new int[MAX_NUM_PLAYERS];
        private int playersAlive = 0;

        private const int MAX_LIVES = 31;
        private const int MAX_DELAY = 31;
        private const int MAX_NUM_PLAYERS = 6;

        public SlaughterHouse() : base("SlaughterHouse", "Slaughter House", "Kill or be killed", SettingsManager.MatchType.DeathMatch, false, 0)
        {

        }

        public override void Hook()
        {
            PatchGameManager.OnPreAddPlayerKill += OnAddPlayerKill;
            PatchGameManager.OnPostPrepareRound += PrepareRound;
        }

        public override void Unhook()
        {
            PatchGameManager.OnPreAddPlayerKill -= OnAddPlayerKill;
            PatchGameManager.OnPostPrepareRound -= PrepareRound;
        }

        public override void RegisterSettings()
        {
            // header
            string headerId = $"gameMode.{id}.header";
            var header = Modifiers.CloneModifierSetting(headerId, name, "ui_boomerangs", "ui_label_friendlyfire");

            // lives slider
            string livesId = $"gameMode.{id}.playerLives";
            var playerLives = Modifiers.CloneModifierSetting(livesId, "Lives", "ui_label_edgeprotection", headerId);

            string[] livesOptions = new string[MAX_LIVES];
            string[] livesHints = new string[MAX_LIVES];
            livesOptions[0] = "Infinite";
            livesHints[0] = "Players can never die";
            for (int i = 1; i < MAX_LIVES; i++)
            {
                livesOptions[i] = i.ToString();
                if (i == 1)
                {
                    livesHints[i] = "Die after being killed 1 time. Seems kind of pointless, doesn't it?";
                    continue;
                }
                livesHints[i] = $"Die after being killed {i} times";
            }
            // default to infinite lives
            playerLives.SetSliderOptions(livesOptions, 0, livesHints);
            // update number of lives on game start
            playerLives.SetGameStartCallback((gameMode, sliderIndex) =>
            {
                // if slider is at 0, set delay to very large number (close to infinite)
                int lives = sliderIndex == 0 ? int.MaxValue / 2 : sliderIndex;
                for (int i = 0; i < MAX_NUM_PLAYERS; i++)
                {
                    // -1 because we want to keep track of lives left, not total lives
                    PlayerLivesArray[i] = lives - 1;
                }
            });

            // delay slider
            string delayId = $"gameMode.{id}.respawnDelay";
            var respawnDelay = Modifiers.CloneModifierSetting(delayId, "Respawn Delay", "ui_label_edgeprotection", headerId);

            string[] delayOptions = new string[MAX_DELAY];
            string[] delayHints = new string[MAX_DELAY];
            delayOptions[0] = "Infinite";
            delayHints[0] = "Players will never respawn. Seems kind of pointless, doesn't it?";
            for (int i = 1; i < MAX_DELAY; i++)
            {
                delayOptions[i] = i.ToString();
                if (i == 1)
                {
                    delayHints[i] = "Respawn after 1 second";
                    continue;
                }
                delayHints[i] = $"Respawn after {i} seconds";
            }
            // default to 1 second respawn delay
            respawnDelay.SetSliderOptions(delayOptions, 1, delayHints);
            // update respawn delay on game start
            respawnDelay.SetGameStartCallback((gameMode, sliderIndex) =>
            {
                // if slider is at 0, set delay to very large number (close to infinite)
                RespawnDelay = sliderIndex == 0 ? int.MaxValue / 2 : sliderIndex;
            });
        }

        // TODO: account for player suicides
        private void PrepareRound(GameManager gameManager)
        {
            PatchGameManager.NumPlayerCheckDisabled = true;
            GetPlayerCount(gameManager);
            ResetPlayerLives();
            BoomerangFoo.Logger.LogInfo("Round prepared!");
        }

        private void GetPlayerCount(GameManager gameManager)
        {
            playersAlive = 0;
            foreach (Player player in gameManager.players)
            {
                if (player != null)
                {
                    playersAlive++;
                    BoomerangFoo.Logger.LogInfo($"Player {player.playerID} is alive!");
                }
            }
            BoomerangFoo.Logger.LogInfo($"There are {playersAlive} players alive!");
        }

        private void ResetPlayerLives()
        {
            for (int i = 0; i < MAX_NUM_PLAYERS; i++)
            {
                PlayerLivesArray[i] = PlayerLives - 1;
            }
        }

        private void OnAddPlayerKill(GameManager gameManager, Player killer, Player killed)
        {
            // debug logging
            BoomerangFoo.Logger.LogInfo($"Player {killer.playerID} killed player {killed.playerID}!");
            BoomerangFoo.Logger.LogInfo($"Player {killer.playerID} has {killer.killsThisRound} kills this round!");

            // respawn killed player if lives left
            PlayerLivesArray[killed.playerID]--;
            if (PlayerLivesArray[killed.playerID] < 0)
            {
                BoomerangFoo.Logger.LogInfo($"Player {killed.playerID} has no more lives!");
                playersAlive--;
                if (playersAlive == 1)
                {
                    BoomerangFoo.Logger.LogInfo($"Player {killer.playerID} wins!");
                    // force end round here
                    var endRoundMethod = typeof(GameManager).GetMethod("CheckPlayersLeftStanding", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (endRoundMethod != null)
                    {
                        // enable player check
                        PatchGameManager.NumPlayerCheckDisabled = false;
                        object[] parameters = new object[] { };
                        endRoundMethod.Invoke(gameManager, parameters);
                    }
                    else
                    {
                        BoomerangFoo.Logger.LogError("Failed to find CheckPlayersLeftStanding method!");
                    }
                }
                return;
            }
            BoomerangFoo.Logger.LogInfo($"There are {playersAlive} players left remaining!");

            // yay has lives left
            BoomerangFoo.Logger.LogInfo($"Player {killed.playerID} has {PlayerLivesArray[killed.playerID]} lives left!");
            gameManager.RespawnPlayer(killed, RespawnDelay);
        }
    }
}