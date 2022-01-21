using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace ProjectMorziva.Harmony.Patches
{
    [HarmonyPatch(typeof(JobUtility))]
    [HarmonyPatch("TryStartErrorRecoverJob")]
    public class PatchTryStartErrorRecoverJob
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn pawn, Exception exception, JobDriver concreteDriver)
        {
            if (concreteDriver is JobDriver_DoBill && pawn.health.Dead && exception is NullReferenceException)
            {
                return false;
            }

            return true;
        }
    }
}