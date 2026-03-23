// bb = RenderItem
// TMI calls l.a(fontRenderer, renderEngine, itemStack, x, y) to render item icon
// and l.b(fontRenderer, renderEngine, itemStack, x, y) to render item overlay (count/durability)

[IKVM.Attributes.NoPackagePrefix]
public class bb
{
    // Delegates set by BetaSharp.Client
    public static Action<sj, ji, iz, int, int>? RenderItemIntoGUIDelegate;
    public static Action<sj, ji, iz, int, int>? RenderItemOverlayDelegate;

    // renderItemIntoGUI — Java name: a(sj, ji, iz, I, I)V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_renderItemIntoGUI(sj fontRenderer, ji renderEngine, iz itemStack, int x, int y)
    {
        RenderItemIntoGUIDelegate?.Invoke(fontRenderer, renderEngine, itemStack, x, y);
    }

    // renderItemOverlayIntoGUI — Java name: b(sj, ji, iz, I, I)V
    [IKVM.Attributes.NameSigAttribute("b", "")]
    public virtual void func_renderItemOverlay(sj fontRenderer, ji renderEngine, iz itemStack, int x, int y)
    {
        RenderItemOverlayDelegate?.Invoke(fontRenderer, renderEngine, itemStack, x, y);
    }
}
