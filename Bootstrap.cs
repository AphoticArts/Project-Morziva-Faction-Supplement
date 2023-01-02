namespace Morziva;

[StaticConstructorOnStartup]
public class Bootstrap
{
    static Bootstrap()
    {
        Harmony.Harmony.ApplyPatches();
    }
}