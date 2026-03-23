// id = GuiContainer extends da (GuiScreen)
// TMI's id.class from the mod jar REPLACES this stub.
// This exists as a fallback and to register the type with MCClassLoader.

[IKVM.Attributes.NoPackagePrefix]
public class id : da
{
    public new int a = 176; // xSize (hides any inherited 'a' — but parent uq uses NameSig)
    public int i = 166;     // ySize
    public dw j;            // container

    public id(dw container)
    {
        j = container;
    }

    // drawBackgroundLayer — Java name: a(F)V
    [IKVM.Attributes.NameSigAttribute("a", "")]
    public virtual void func_drawBackgroundLayer(float partialTicks) { }

    // drawForegroundLayer — Java name: k()V
    public virtual void k() { }
}
