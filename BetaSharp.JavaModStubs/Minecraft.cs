namespace net.minecraft.client;

[IKVM.Attributes.NoPackagePrefix]
public class Minecraft
{

    internal object? _game;
    internal object? _world;
    public World? theWorld => _world as World;

    public static java.io.File a(string s)
    {
        return null;
    }
}