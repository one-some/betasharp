using System.Reflection;
using HarmonyLib;
using IKVM.Attributes;

namespace BetaSharp.Client.Modding.Java;

public static class IkvmPatches
{
    private static Harmony? _harmony;

    public static void Apply()
    {
        _harmony = new Harmony("com.betasharp.ikvmpatches");

        var managedType = Type.GetType("IKVM.Runtime.RuntimeManagedJavaType, IKVM.Runtime")!;

        var makeMethodDescriptor = managedType.GetMethod(
            "MakeMethodDescriptor",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly
        )!;

        var postfix = new HarmonyMethod(typeof(IkvmPatches), nameof(MakeMethodDescriptor_Postfix));
        _harmony.Patch(makeMethodDescriptor, postfix: postfix);

        var createFieldWrapper = managedType.GetMethod(
            "CreateFieldWrapperDotNet",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly
        )!;

        var prefix = new HarmonyMethod(typeof(IkvmPatches), nameof(CreateFieldWrapperDotNet_Prefix));
        _harmony.Patch(createFieldWrapper, prefix: prefix);

        // Patch GetFieldWrapper to log missed field lookups (useful for finding expected signatures)
        var javaTypeBase = managedType.BaseType!;
        while (javaTypeBase != null && javaTypeBase.Name != "RuntimeJavaType")
            javaTypeBase = javaTypeBase.BaseType!;

        var getFieldWrapper = javaTypeBase.GetMethod(
            "GetFieldWrapper",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            null,
            [typeof(string), typeof(string)],
            null
        );

        if (getFieldWrapper != null)
        {
            var gfwPostfix = new HarmonyMethod(typeof(IkvmPatches), nameof(GetFieldWrapper_Postfix));
            _harmony.Patch(getFieldWrapper, postfix: gfwPostfix);
        }
    }

    static void MakeMethodDescriptor_Postfix(bool __result, MethodBase mb, ref string name, ref string sig)
    {
        if (!__result)
            return;

        var attr = mb.GetCustomAttribute<NameSigAttribute>();
        if (attr == null)
            return;

        if (!string.IsNullOrEmpty(attr.Name))
            name = attr.Name;
    }

    static void GetFieldWrapper_Postfix(object __instance, string fieldName, string fieldSig, object? __result)
    {
        if (__result == null && __instance.GetType().Name == "RuntimeManagedJavaType")
            Console.WriteLine($"[IkvmPatches] Missing field: {__instance}.{fieldName} sig={fieldSig}");
    }

    static void CreateFieldWrapperDotNet_Prefix(ref string name, FieldInfo field)
    {
        var attr = field.GetCustomAttribute<NameSigAttribute>();
        if (attr != null && !string.IsNullOrEmpty(attr.Name))
        {
            name = attr.Name;
        }
    }
}
