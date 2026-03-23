using net.minecraft.client;

[IKVM.Attributes.NoPackagePrefix]
public class BaseMod
{
    public virtual bool OnTickInGame(Minecraft mc)
    {
        Console.WriteLine("overwrite me or die");
        return true;
    }

    public virtual string Version()
    {
        return "?";
    }
}