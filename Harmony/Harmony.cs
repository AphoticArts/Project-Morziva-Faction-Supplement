namespace Morziva.Harmony;

public class Harmony
{
    private static HarmonyLib.Harmony harmonyInstance = new ("com.morziva.fs");

    public static void ApplyPatches() => harmonyInstance.PatchAll();
}