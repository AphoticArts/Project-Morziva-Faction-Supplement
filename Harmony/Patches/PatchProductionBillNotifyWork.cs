using Morziva.Comps;
using Morziva.Defs;

namespace Morziva.Harmony.Patches;

[HarmonyPatch(typeof(Bill), nameof(Notify_PawnDidWork))]
public class PatchProductionBillNotifyWork
{
    [HarmonyPostfix]
    public static void Notify_PawnDidWork(Pawn p, Bill_Production __instance)
    {
        if (!__instance.recipe.UsesUnfinishedThing)
            return;
        var reductionFactor = 1f;
        Thing reducerEquipment =
            p.equipment.AllEquipmentListForReading.FirstOrDefault(t =>
                t.def.HasModExtension<ToxicityReducerExtension>());
        var reducerTrait =
            p.story.traits.allTraits.FirstOrDefault(t => t.def.HasModExtension<ToxicityReducerExtension>());
        if (reducerEquipment != null)
        {
            reductionFactor -= reducerEquipment.def.GetModExtension<ToxicityReducerExtension>()
                .toxicityReductionFactor;
        }

        if (reducerTrait is not null)
        {
            reductionFactor -= reducerTrait.def.GetModExtension<ToxicityReducerExtension>()
                .toxicityReductionFactor;
        }


        if (p.jobs.curJob?.def != JobDefOf.DoBill)
            return;
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