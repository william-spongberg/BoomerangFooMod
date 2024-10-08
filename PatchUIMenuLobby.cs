﻿using HarmonyLib;

namespace BoomerangFoo
{
    [HarmonyPatch(typeof(UIMenuLobby), nameof(UIMenuLobby.GetClosestAvailableGridCell))]
    class UIMenuLobbyGetClosestAvailableGridCellPatch
    {
        static UICharacterGridCell.SelectionState[] selections;
        static void Prefix(UIMenuLobby __instance)
        {
            if (_CustomSettings.EnableDuplicatedCharacters)
            {
                if (selections == null || selections.Length < __instance.characterGridCells.Length) {
                    selections = new UICharacterGridCell.SelectionState[__instance.characterGridCells.Length];
                }
                for (int i = 0; i < __instance.characterGridCells.Length; i++)
                {
                    var cell = __instance.characterGridCells[i];
                    selections[i] = cell.selectionState;
                    cell.selectionState = UICharacterGridCell.SelectionState.Deselected;
                }
            }
        }

        static void Postfix(UIMenuLobby __instance)
        {
            if (_CustomSettings.EnableDuplicatedCharacters)
            {
                for (int i = 0; i < __instance.characterGridCells.Length; i++)
                {
                    var cell = __instance.characterGridCells[i];
                    cell.selectionState = selections[i];
                }
            }
        }
    }

    [HarmonyPatch(typeof(UIMenuLobby), "IsGridCellAvailable")]
    class UIMenuLobbyIsGridCellAvailablePatch
    {
        static bool Prefix(ref bool __result)
        {
            // if duplicated characters enabled, then don't block
            if (_CustomSettings.EnableDuplicatedCharacters)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
