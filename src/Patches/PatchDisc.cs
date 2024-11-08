using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using BoomerangFoo.Powerups;

namespace BoomerangFoo.Patches
{
    [HarmonyPatch(typeof(Disc), nameof(Disc.MultiDiscBurst))]
    class DiscMultiDiscBurstPatch
    {
        public static int GetMultiBoomerangSplit(Disc instance)
        {
            PlayerState playerState = CommonFunctions.GetPlayerState(instance.DiscOwner);
            if (instance.discPowerup.HasPowerup(PowerupType.ExplosiveDisc) || instance.discPowerup.HasPowerup(PowerupType.FireDisc))
            {
                return Math.Max(playerState.multiBoomerangSplit - 1, 1);
            }
            return Math.Max(playerState.multiBoomerangSplit, 1);
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var discPowerupField = AccessTools.Field(typeof(Disc), "discPowerup");
            var myMethod = AccessTools.Method(typeof(DiscMultiDiscBurstPatch), nameof(DiscMultiDiscBurstPatch.GetMultiBoomerangSplit));

            int startIdx = -1;
            for (int i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                if (code.opcode == OpCodes.Ldc_I4_S && code.operand is sbyte value && value == 32)
                {
                    startIdx = i-1;
                    break;
                }
            }

            if (startIdx == -1)
            {
                BoomerangFoo.Logger.LogError($"Disc ldfld was not found");
                return instructions;
            }

            codes[startIdx] = new CodeInstruction(OpCodes.Call, myMethod);

            for (int i = startIdx + 1; i < codes.Count; i++)
            {
                var code = codes[i];
                if (code.opcode == OpCodes.Stloc_0)
                {
                    break;
                }
                codes[i] = new CodeInstruction(OpCodes.Nop);
            }

            return codes.AsEnumerable();
        }
    }
}
