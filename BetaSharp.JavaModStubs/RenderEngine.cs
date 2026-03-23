// ji = RenderEngine (TextureManager)
// TMI calls this.b.p.b(string) to get texture ID, and this.b.p.b(int) to bind texture

[IKVM.Attributes.NoPackagePrefix]
public class ji
{
    // Delegates set by BetaSharp.Client
    public static Func<string, int>? GetTextureDelegate;
    public static Action<int>? BindTextureDelegate;

    // getTexture — Java name: b(Ljava/lang/String;)I
    [IKVM.Attributes.NameSigAttribute("b", "")]
    public virtual int func_getTexture(string path)
    {
        return GetTextureDelegate?.Invoke(path) ?? 0;
    }

    // bindTexture — Java name: b(I)V
    [IKVM.Attributes.NameSigAttribute("b", "")]
    public virtual void func_bindTexture(int textureId)
    {
        BindTextureDelegate?.Invoke(textureId);
    }
}
