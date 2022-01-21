using ProjectMorziva.Harmony;
using Verse;

namespace ProjectMorziva
{
    [StaticConstructorOnStartup]
    public class Bootstrap
    {
        static Bootstrap()
        {
            HarmonyBase.ApplyPatches();
        }
    }
}