using ProjectMorziva.Utils;
using UnityEngine;
using Verse;

namespace ProjectMorziva.Comps
{
    public class CompProperties_HazardousMaterial : CompProperties
    {
        public HediffDef hediffToApply;
        public int workbenchContaminationInterval = 250;
        public float baseContaminationSeverity = 0.01f;

        public CompProperties_HazardousMaterial()
        {
            this.compClass = typeof(Comp_HazardousMaterial);
        }
    }
    
    public class Comp_HazardousMaterial : ThingComp
    {
        public CompProperties_HazardousMaterial Props => (CompProperties_HazardousMaterial)this.props;
        
        private float lastWorkAmountSeen = -1f;
        private int ticksSinceLastContamination = 0;

        public void TryContamination(Pawn target, float severityFactor, int stackCount = 1, bool isWorkbench = true)
        {
            ticksSinceLastContamination++;
            if (isWorkbench && ticksSinceLastContamination >= this.Props.workbenchContaminationInterval)
            {
                Log.Message($"Contaminating (stack size {stackCount})");
                HediffUtils.AddOrUpdateHediffWithSeverity(target, this.Props.hediffToApply, null, stackCount * this.Props.baseContaminationSeverity);
                ticksSinceLastContamination = 0;
            }    
        }

        public void TryWorkContamination(Pawn target, float severityFactor, int stackCount, float workLeft)
        {
            int workUnitsElapsed = Mathf.RoundToInt(lastWorkAmountSeen - workLeft);
            if (workUnitsElapsed >= 1)
            {
                lastWorkAmountSeen = workLeft;
                for (int i = 0; i < workUnitsElapsed; i++)
                {
                    TryContamination(target, severityFactor, stackCount, true);
                }
            }
            else if (lastWorkAmountSeen == -1f)
            {
                lastWorkAmountSeen = workLeft;
            }    
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref ticksSinceLastContamination, "ticksSinceLastContamination", 0);
        }
    }
}