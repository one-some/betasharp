// u = RenderHelper
// TMI calls u.b() to enable standard item lighting, u.a() to disable it

[IKVM.Attributes.NoPackagePrefix]
public class u
{
    // Delegates set by BetaSharp.Client
    public static Action? EnableStandardItemLightingDelegate;
    public static Action? DisableStandardItemLightingDelegate;

    // disableStandardItemLighting — Java name: a()V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public static void func_disableStandardItemLighting()
    {
        DisableStandardItemLightingDelegate?.Invoke();
    }

    // enableStandardItemLighting — Java name: b()V
    [IKVM.Attributes.NameSigAttribute("b", "")]
    public static void func_enableStandardItemLighting()
    {
        EnableStandardItemLightingDelegate?.Invoke();
    }
}
