namespace Morziva.Harmony.Patches;

[HarmonyPatch(typeof(JobUtility), nameof(TryStartErrorRecoverJob))]
public class PatchTryStartErrorRecoverJob
{
    [HarmonyPrefix]
    public static bool TryStartErrorRecoverJob(Pawn pawn, Exception exception, JobDriver concreteDriver) =>
        concreteDriver is not JobDriver_DoBill ||
        exception is not NullReferenceException ||
        !pawn.health.Dead;
}