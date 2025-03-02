using BoomerangFoo.Patches;
using BoomerangFoo.UI;
using System.Reflection;

namespace BoomerangFoo.GameModes
{
    public class SlaughterHouse : GameMode
    {

        public int PlayerLives = 1;
        public float RespawnDelay = 1f;

        private int[] PlayerLivesArray = new int[MAX_NUM_PLAYERS];
        private int playersAlive = 0;
        private bool endingRound = false;

        private const int MAX_LIVES = 10;
        private const int MAX_DELAY = 60;
        private const int MAX_NUM_PLAYERS = 6;

        public SlaughterHouse() : base("SlaughterHouse", "Slaughter House", "Kill or be killed", SettingsManager.MatchType.DeathMatch, false, 0)
        {

        }

        public override void Hook()
        {
            PatchGameManager.OnPreAddPlayerKill += OnAddPlayerKill;
            PatchGameManager.OnPostPrepareRound += PrepareRound;
            PatchGameManager.OnPostUpdate += CheckPostUpdate;
        }

        public override void Unhook()
        {
            PatchGameManager.OnPreAddPlayerKill -= OnAddPlayerKill;
            PatchGameManager.OnPostPrepareRound -= PrepareRound;
            PatchGameManager.OnPostUpdate -= CheckPostUpdate;
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
            livesHints[0] = "Players can never die. Play until kill limit reached!";
            // minimum 2 lives
            for (int i = 1; i < MAX_LIVES - 1; i++)
            {
                livesOptions[i] = (i + 1).ToString();
                livesHints[i] = $"Die after being killed {i + 1} times";
            }
            // default to infinite lives
            playerLives.SetSliderOptions(livesOptions, 0, livesHints);
            // update number of lives on game start
            playerLives.SetGameStartCallback((gameMode, sliderIndex) =>
            {
                // if slider is at 0, set delay to very large number (close to infinite)
                PlayerLives = sliderIndex == 0 ? int.MaxValue / 2 : sliderIndex + 1;
            });

            // delay slider
            string delayId = $"gameMode.{id}.respawnDelay";
            var respawnDelay = Modifiers.CloneModifierSetting(delayId, "Respawn Delay", "ui_label_edgeprotection", headerId);

            string[] delayOptions = new string[MAX_DELAY];
            string[] delayHints = new string[MAX_DELAY];
            for (int i = 0; i < MAX_DELAY; i++)
            {
                delayOptions[i] = i.ToString();
                if (i == 0)
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
                RespawnDelay = sliderIndex;
            });
        }

        private void CheckPostUpdate(GameManager gameManager)
        {
            CheckPlayersAlive(gameManager);
        }

        private void PrepareRound(GameManager gameManager)
        {
            endingRound = false;
            PatchGameManager.NumPlayerCheckDisabled = true;
            GetPlayerCount(gameManager);
            ResetPlayerLives(gameManager);
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

        private void ResetPlayerLives(GameManager gameManager)
        {
            foreach (Player player in gameManager.players)
            {
                PlayerLivesArray[player.playerID] = PlayerLives;
            }
            BoomerangFoo.Logger.LogInfo($"Players have {PlayerLives} lives!");
        }

        private void OnAddPlayerKill(GameManager gameManager, Player killer, Player killed)
        {
            // debug logging
            BoomerangFoo.Logger.LogInfo($"Player {killer.playerID} killed player {killed.playerID}!");
            BoomerangFoo.Logger.LogInfo($"Player {killer.playerID} has {killer.killsThisRound} kills this round!");

            // respawn killed player if lives left
            PlayerLivesArray[killed.playerID]--;
            if (PlayerLivesArray[killed.playerID] <= 0)
            {
                BoomerangFoo.Logger.LogInfo($"Player {killed.playerID} has no more lives!");
                playersAlive--;
                CheckEndRound(gameManager);
                return;
            }
            BoomerangFoo.Logger.LogInfo($"There are {playersAlive} players left remaining!");

            // yay has lives left
            BoomerangFoo.Logger.LogInfo($"Player {killed.playerID} has {PlayerLivesArray[killed.playerID]} lives left!");
            gameManager.RespawnPlayer(killed, RespawnDelay);
        }

        private void CheckEndRound(GameManager gameManager)
        {
            // if only one player is alive, end the round
            if (playersAlive <= 1 && gameManager.OpponentsLeftStandingNow() <= 1 && !endingRound)
            {
                // force end round here
                var endRoundMethod = typeof(GameManager).GetMethod("CheckPlayersLeftStanding", BindingFlags.NonPublic | BindingFlags.Instance);
                if (endRoundMethod == null)
                {
                    BoomerangFoo.Logger.LogError("Failed to find CheckPlayersLeftStanding method!");
                }
                else
                {
                    // stop checking for ending round, enable player check
                    endingRound = true;
                    PatchGameManager.NumPlayerCheckDisabled = false;

                    // force check players left standing
                    object[] parameters = new object[] { };
                    endRoundMethod.Invoke(gameManager, parameters);
                    BoomerangFoo.Logger.LogInfo($"{playersAlive} player left standing, ending round!");
                    BoomerangFoo.Logger.LogInfo($"There are {gameManager.OpponentsLeftStandingNow()} opponents left standing!");
                }
            }
        }

        private void CheckPlayersAlive(GameManager gameManager)
        {
            int alivePlayers = 0;
            // check if any dead players have lives left
            foreach (Player player in gameManager.players)
            {
                if (player)
                {
                    // check if player commited suicide
                    if (!player.isSpawningIn)
                    {
                        if (player.actorState == Actor.ActorState.Dead)
                        {
                            // check if has more than one life remaining
                            if (PlayerLivesArray[player.playerID] > 1)
                            {
                                // respawn player, shouldn't remain dead
                                BoomerangFoo.Logger.LogInfo($"Player {player.playerID} is dead but has {PlayerLivesArray[player.playerID]} lives left, respawning!");
                                PlayerLivesArray[player.playerID]--;
                                gameManager.RespawnPlayer(player, RespawnDelay);
                                alivePlayers++;
                                player.isSpawningIn = true;
                            }
                            else
                            {
                                // player will remain dead, check if end round condition met
                                CheckEndRound(gameManager);
                            }
                        }
                        // alive now, reset flag
                        else
                        {
                            player.isSpawningIn = false;
                        }
                    }
                    else
                    {
                        alivePlayers++;
                    }
                }
            }
            // update players alive
            playersAlive = alivePlayers;
        }
    }
}