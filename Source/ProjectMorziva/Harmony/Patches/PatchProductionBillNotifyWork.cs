using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using ProjectMorziva.Comps;
using ProjectMorziva.Defs;
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
                float reductionFactor = 1f;
                Thing reducerEquipment =
                    p.equipment.AllEquipmentListForReading.FirstOrDefault(t =>
                        t.def.HasModExtension<ToxicityReducerExtension>());
                Trait reducerTrait =
                    p.story.traits.allTraits.FirstOrDefault(t => t.def.HasModExtension<ToxicityReducerExtension>());
                if (reducerEquipment != null)
                {
                    reductionFactor -= reducerEquipment.def.GetModExtension<ToxicityReducerExtension>()
                        .toxicityReductionFactor;
                }

                if (reducerTrait != null)
                {
                    reductionFactor -= reducerTrait.def.GetModExtension<ToxicityReducerExtension>()
                        .toxicityReductionFactor;
                }
                
                
                if (p.jobs.curJob?.def == JobDefOf.DoBill)
                {
                    if (p.jobs.curJob.GetTarget(TargetIndex.A).Thing is Building building &&
                        building.def.HasModExtension<ToxicityReducerExtension>())
                    {
                        reductionFactor -= building.def.GetModExtension<ToxicityReducerExtension>()
                            .toxicityReductionFactor;
                    }
                    
                    if (reductionFactor > 0f && p.jobs.curJob.GetTarget(TargetIndex.B).Thing is UnfinishedThing unfinishedThing)
                    {
                        unfinishedThing.ingredients.ForEach(thing =>
                            thing.TryGetComp<Comp_HazardousMaterial>()?.TryWorkContamination(p, reductionFactor, thing.stackCount,
                                Mathf.CeilToInt(unfinishedThing.workLeft / 60f)));
                    }
                }    
            }
        }
    }
}