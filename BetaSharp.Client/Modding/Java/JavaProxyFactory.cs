using System.Reflection;
using System.Reflection.Emit;

namespace BetaSharp.Client.Modding.Java;

public static class JavaProxyFactory
{
    private static readonly ModuleBuilder _mb;
    private static readonly Dictionary<string, Type> _cache = new();

    static JavaProxyFactory()
    {
        var ab = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName("BetaSharp.JavaModProxies"),
            AssemblyBuilderAccess.Run
        );
        _mb = ab.DefineDynamicModule("BetaSharp.JavaModProxies");
    }

    public static Type CreateProxy(string javaName, Type realType)
    {
        if (realType.FullName == javaName)
            return realType;

        if (_cache.TryGetValue(javaName, out var cached))
            return cached;
        
        var tb = _mb.DefineType(javaName, TypeAttributes.Public, realType);

        var ctor = typeof(IKVM.Attributes.NoPackagePrefixAttribute)
            .GetConstructor(Type.EmptyTypes)!;
        tb.SetCustomAttribute(new CustomAttributeBuilder(ctor, []));

        var proxyType = tb.CreateType()!;
        _cache[javaName] = proxyType;
        return proxyType;
    }
}