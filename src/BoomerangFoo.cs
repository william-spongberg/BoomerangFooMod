using BepInEx;
using BepInEx.Logging;
using BoomerangFoo.Powerups;
using HarmonyLib;

namespace BoomerangFoo;

[BepInPlugin("Jeffjewett27.plugins.BoomerangFoo", "BoomerangFoo", "0.2.1.0")]
public class BoomerangFoo : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        var harmony = new Harmony("Jeffjewett27.patch");
        harmony.PatchAll();

        // Register powerup hooks
        BamboozlePowerup.Register();
        DecoyPowerup.Register();
        FirePowerup.Register();
        MoveFasterPowerup.Register();
        FlyingPowerup.Register();
    }
}
