namespace ProjectMorziva.Harmony
{
    public class HarmonyBase
    {
        private static HarmonyLib.Harmony harmonyInstance = new HarmonyLib.Harmony("com.morziva.patch");

        public static void ApplyPatches()
        {
            harmonyInstance.PatchAll();
        }
    }
}