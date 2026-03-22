using net.minecraft.client;

[IKVM.Attributes.NoPackagePrefix]
public class ModLoader
{
    private static Dictionary<BaseMod, bool> inGameHooks = new();

    public static void OnTick(Minecraft mc)
    {
        Console.WriteLine("DOne output doneeee");
        if (mc.theWorld != null)
        {
            foreach (var item in inGameHooks)
            {
                if (!item.Value && !item.Key.OnTickInGame(mc))
                {
                    inGameHooks.Remove(item.Key);
                }
            }
        }
    }

    public static void SetInGameHook(BaseMod mod, bool flag, bool flag1)
    {
        if (flag)
        {
            inGameHooks[mod] = flag1;
        } else
        {
            inGameHooks.Remove(mod);
        }
    }
}