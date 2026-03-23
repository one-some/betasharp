// sj = FontRenderer
// TMI calls g.a(text, x, y, color) for drawing text and g.a(text) for string width

[IKVM.Attributes.NoPackagePrefix]
public class sj
{
    // Delegates set by BetaSharp.Client
    public static Action<string, int, int, int>? DrawStringDelegate;
    public static Func<string, int>? GetStringWidthDelegate;

    // drawStringWithShadow — Java name: a(Ljava/lang/String;III)V (void in original MC)
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_drawString(string text, int x, int y, int color)
    {
        DrawStringDelegate?.Invoke(text, x, y, color);
    }

    // getStringWidth — Java name: a(Ljava/lang/String;)I
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual int func_getStringWidth(string text)
    {
        return GetStringWidthDelegate?.Invoke(text) ?? 0;
    }
}
