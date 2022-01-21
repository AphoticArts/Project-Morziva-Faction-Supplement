using System.Collections.Generic;
using HarmonyLib;
using ProjectMorziva.Comps;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ProjectMorziva.Harmony.Patches
{
    [HarmonyPatch(typeof(Bill))]
    [HarmonyPatch("Notify_PawnDidWork")]
    public class PatchProductionBillNotifyWork
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn p, Bill_Production __instance)
        {
            if (__instance.recipe.UsesUnfinishedThing)
            {
                if (p.jobs.curJob?.def == JobDefOf.DoBill &&
                    p.jobs.curJob.GetTarget(TargetIndex.B).Thing is UnfinishedThing unfinishedThing)
                {
                    unfinishedThing.ingredients.ForEach(thing => thing.TryGetComp<Comp_HazardousMaterial>()?.TryWorkContamination(p, thing.stackCount,Mathf.CeilToInt(unfinishedThing.workLeft / 60f)));
                }    
            }
        }
    }
}