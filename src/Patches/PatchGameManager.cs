using System;
using HarmonyLib;
using BoomerangFoo.GameModes;

namespace BoomerangFoo.Patches
{
    public class PatchGameManager
    {
        //public delegate void OnPostUpdateHandler(GameManager gameManager);
        //public static event OnPostUpdateHandler OnPostUpdate;
        public static event Action<GameManager> OnPostUpdate;
        public static void InvokePostUpdate(GameManager gameManager) { OnPostUpdate?.Invoke(gameManager); }

        public static event Action<GameManager> OnPostPrepareRound;
        public static void InvokePostPrepareRound(GameManager gameManager) { OnPostPrepareRound?.Invoke(gameManager); }

        public static event Action<GameManager> OnPreStartMatch;
        public static void InvokePreStartMatch(GameManager gameManager) { OnPreStartMatch?.Invoke(gameManager); }

        public static event Action<GameManager> OnPreStartRoundSequence;
        public static void InvokePreStartRoundSequence(GameManager gameManager) { OnPreStartRoundSequence?.Invoke(gameManager); }

        public static event Action<GameManager> OnPreCheckPlayersLeftStanding;
        public static void InvokePreCheckPlayersLeftStanding(GameManager gameManager) { OnPreCheckPlayersLeftStanding?.Invoke(gameManager); }

        public static Func<GameManager, int> OpponentsLeftStandingNow;

        public static event Action<GameManager, Player, Player> OnPreAddPlayerKill;
        public static void InvokePreAddPlayerKill(GameManager gameManager, Player killer, Player killed) { OnPreAddPlayerKill?.Invoke(gameManager, killer, killed); }

        public static Func<GameManager, Player, Player, GameMode.Relationship> PlayerRelationship; 

    }

    [HarmonyPatch(typeof(GameManager), "Update")]
    class GameManagerUpdatePatch
    {

        static void Postfix(GameManager __instance)
        {
            PatchGameManager.InvokePostUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(GameManager), "PrepareRound")]
    class GameManagerPrepareRoundPatch
    {

        static void Postfix(GameManager __instance)
        {
            PatchGameManager.InvokePostPrepareRound(__instance);
            if (_CustomSettings.SuddenDeathTimeLimit > 0)
            {
                Singleton<SettingsManager>.Instance.MatchSettings.suddenDeathTime = _CustomSettings.SuddenDeathTimeLimit;
            }
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartMatch))]
    class GameManagerStartMatchPatch
    {

        static void Prefix(GameManager __instance)
        {
            PatchGameManager.InvokePreStartMatch(__instance);
        }
    }

    [HarmonyPatch(typeof(GameManager), "StartRoundSequence")]
    class GameManagerStartRoundSequencePatch
    {

        static void Prefix(GameManager __instance)
        {
            PatchGameManager.InvokePreStartRoundSequence(__instance);
        }
    }

    [HarmonyPatch(typeof(GameManager), "CheckPlayersLeftStanding")]
    class GameManagerCheckPlayersLeftStandingPatch
    {

        static void Prefix(GameManager __instance)
        {
            PatchGameManager.InvokePreCheckPlayersLeftStanding(__instance);
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.OpponentsLeftStandingNow))]
    class GameManagerOpponentsLeftStandingNowPatch
    {

        static bool Prefix(GameManager __instance, ref int __result)
        {
            if (PatchGameManager.OpponentsLeftStandingNow != null)
            {
                int oppLeft = PatchGameManager.OpponentsLeftStandingNow(__instance);
                __result = oppLeft;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AddGoldenDiscPoint))]
    class GameManagerAddGoldenDiscPointPatch
    {
        // This function is hardcoded to use the playerID rather than the RowID that accounts for teams.
        static int winningPlayerId;

        static void Prefix(GameManager __instance, Player winningPlayer)
        {
            winningPlayerId = winningPlayer.playerID;
            winningPlayer.playerID = __instance.GetRowID(winningPlayer);
        }

        static void Postfix(Player winningPlayer)
        {
            winningPlayer.playerID = winningPlayerId;
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.AddPlayerKill))]
    class GameManagerAddPlayerKillPatch
    {
        public static Player killedPlayer;
        private static GameMode.Relationship relationship;
        private static int teamKills;

        static void Prefix(GameManager __instance, Player player, ref bool killerWasOpponent)
        {
            // killedPlayer needs to be set from pre_Player.Die
            if (killedPlayer == null) return;

            Player killer = player;
            PatchGameManager.InvokePreAddPlayerKill(__instance, killer, killedPlayer);

            if (PatchGameManager.PlayerRelationship != null)
            {
                relationship = PatchGameManager.PlayerRelationship(__instance, killer, killedPlayer);
                killerWasOpponent = relationship == GameMode.Relationship.Opponent;
            } else
            {
                relationship = killerWasOpponent ? GameMode.Relationship.Opponent : GameMode.Relationship.Teammate;
            }
            int rowID = __instance.GetRowID(killer);
            teamKills = __instance.matchScores[rowID].TeamKills;

            killedPlayer = null;
        }

        static void Postfix(GameManager __instance, Player player)
        {
            int rowID = __instance.GetRowID(player);
            if (relationship != GameMode.Relationship.Teammate)
            {
                __instance.matchScores[rowID].TeamKills = teamKills;
            }
        }
    }
}
