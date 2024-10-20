using BepInEx;
using BepInEx.Logging;
using BoomerangFoo.GameModes;
using BoomerangFoo.Patches;
using BoomerangFoo.Powerups;
using HarmonyLib;

namespace BoomerangFoo;

[BepInPlugin("Jeffjewett27.plugins.BoomerangFoo", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class BoomerangFoo : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");

        var harmony = new Harmony("Jeffjewett27.patch");
        harmony.PatchAll();

        // Register powerup hooks
        BamboozlePowerup.Register();
        DecoyPowerup.Register();
        FirePowerup.Register();
        MoveFasterPowerup.Register();
        FlyingPowerup.Register();

        GameMode.Register(new GameMode("Deathmatch", "Free For All", "Everyone is an enemy", SettingsManager.MatchType.DeathMatch, false, 0), GameMode.Slot.Deathmatch);
        GameMode.Register(new GameMode("TeamDeathmatch", "Team Up", "Play in teams", SettingsManager.MatchType.DeathMatch, true, 1), GameMode.Slot.TeamUp);
        GameMode.Register(new GameMode("HideAndSeek", "Hide And Seek", "Find your foes", SettingsManager.MatchType.HideAndSeek, false, 2), GameMode.Slot.HideAndSeek);
        GameMode.Register(new GameMode("GoldenBoomerang", "Golden Boomerang", "Hold onto the golden boomerang", SettingsManager.MatchType.GoldenDisc, false, 3), GameMode.Slot.GoldenBoomerang);
        PatchUIMenuMatchSettings.OnMatchTypeSelected += GameMode.MatchSelected;

        GameMode.Register(new PowerDrain(), GameMode.Slot.Extra1);
        GameMode.Register(new RamboHulk(), GameMode.Slot.Extra2);
    }
}
