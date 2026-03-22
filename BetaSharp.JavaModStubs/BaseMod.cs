using net.minecraft.client;

[IKVM.Attributes.NoPackagePrefix]
public class BaseMod
{
    public virtual bool OnTickInGame(Minecraft mc)
    {
        // TODO
        return true;
    }

    public virtual string Version()
    {
        return "?";
    }
}