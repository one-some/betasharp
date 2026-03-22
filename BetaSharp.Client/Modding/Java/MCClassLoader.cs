extern alias JavaModStubs;

using BetaSharp.Client.Modding.Java;
using java.lang;
using java.net;

public class MCClassLoader : URLClassLoader
{
    private static readonly Dictionary<string, Type> _stubs = new()
    {
        // TODO: ModLoader
        ["BaseMod"] = typeof(JavaModStubs::BaseMod),
        ["ModLoader"] = typeof(JavaModStubs::ModLoader),
        ["net.minecraft.client.Minecraft"] = typeof(JavaModStubs::net.minecraft.client.Minecraft),
        ["iz"] = typeof(JavaModStubs::ItemStack),
    };

    public MCClassLoader() : base([]) {}

    public void AddJar(string path)
    {
        addURL(new URL($"file:{path}"));
    }

    protected override Class findClass(string name)
    {
        Console.WriteLine($"ima looking for {name}");

        if (_stubs.TryGetValue(name, out var nativeType))
        {
            var proxyType = JavaProxyFactory.CreateProxy(name, nativeType);
            return ikvm.runtime.Util.getFriendlyClassFromType(proxyType);
        }

        return base.findClass(name);
    }
}