using HarmonyLib;

namespace BoomerangFoo.Patches
{
    [HarmonyPatch(typeof(UICharacterGridCell), nameof(UICharacterGridCell.BootAllOtherPlayersExceptMe))]
    class UICharacterGridCellBootAllOtherPlayersExceptMePatch
    {
        static bool Prefix()
        {
            // if duplicate characters allowed, stop the blocker from running
            return !_CustomSettings.EnableDuplicatedCharacters;
        }
    }
}
