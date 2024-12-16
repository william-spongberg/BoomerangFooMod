using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BoomerangFoo.Patches
{
    [HarmonyPatch(typeof(UIScoreRow), "RemoveTeamKill")]
    class UIScoreRow_RemoveTeamKill_Patch
    {
        private static readonly FieldInfo uiScorePoints = typeof(UIScoreRow).GetField("uiScorePoints", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo incrementWaitTime = typeof(UIScoreRow).GetField("incrementWaitTime", BindingFlags.Instance | BindingFlags.NonPublic);

        static bool Prefix(UIScoreRow __instance, ref IEnumerator __result)
        {
            // There is a bug in the original implementation, which is usually unreachable
            // Copy the implementation of the suicide point loss
            __result = RemoveTeamKill(__instance);
            return false;
        }

        private static IEnumerator RemoveTeamKill(UIScoreRow instance)
        {
            List<UIScorePoint> scorePoints = (List<UIScorePoint>)(uiScorePoints.GetValue(instance));
            instance.rowScore.TeamKills++;
            if (instance.rowScore.Total > 0 && instance.rowScore.Total <= scorePoints.Count)
            {
                yield return new WaitForSecondsRealtime((float)incrementWaitTime.GetValue(instance));
                instance.rowScore.Total--;
                scorePoints[instance.rowScore.Total].SetActive(active: false);
                scorePoints[instance.rowScore.Total].image.sprite = scorePoints[instance.rowScore.Total].skullSprite;
                Singleton<AudioManager>.Instance.PlayOneShot("event:/ui/remove_point");
            }
        }
    }
}
